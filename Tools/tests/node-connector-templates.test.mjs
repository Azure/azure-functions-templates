import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdir, mkdtemp, readFile, readdir, rm, symlink, writeFile } from 'node:fs/promises';
import { createRequire, stripTypeScriptTypes } from 'node:module';
import os from 'node:os';
import path from 'node:path';
import test from 'node:test';
import { fileURLToPath, pathToFileURL } from 'node:url';

const root = fileURLToPath(new URL('../../', import.meta.url));
const connectors = [
    { name: 'Outlook', label: 'Office 365 trigger', setting: 'OnNewEmailTriggerConfigName', sdk: 'Office365', key: 'OnNewEmail', operation: 'OnNewEmailV3' },
    { name: 'Sharepoint', label: 'SharePoint trigger', setting: 'OnSharepointNewFileTriggerConfigName', sdk: 'Sharepointonline', key: 'OnNewFileItems', operation: 'GetOnNewFileItems' },
    { name: 'OneDrive', label: 'OneDrive trigger', setting: 'OnOneDriveNewFileTriggerConfigName', sdk: 'Onedriveforbusiness', key: 'OnNewFiles', operation: 'OnNewFilesV2' },
    { name: 'Dataverse', label: 'Dataverse trigger', setting: 'OnDataverseRowChangedTriggerConfigName', sdk: 'Commondataservice', key: 'OnNewItems', operation: 'GetOnNewItems_V2' },
    { name: 'Teams', label: 'Teams trigger', setting: 'OnNewChannelMessageTriggerConfigName', sdk: 'Teams', key: 'OnNewChannelMessage', operation: 'OnNewChannelMessage' },
];
const languages = { JavaScript: 'js', TypeScript: 'ts' };
const resourceXml = await readFile(path.join(root, 'Functions.Templates/Resources/Resources.resx'), 'utf8');
const resources = new Set([...resourceXml.matchAll(/<data name="([^"]+)"/g)].map((match) => match[1]));
const catalogs = await Promise.all(['bindings.json', 'bindings-bundle-v4.json'].map(async (name) =>
    JSON.parse(await readFile(path.join(root, 'Functions.Templates/Bindings', name), 'utf8'))));
const packages = await Promise.all(['ExtensionBundleTemplates-4.x.nuspec', 'ExtensionBundlePreviewTemplates-4.x.nuspec'].map((name) =>
    readFile(path.join(root, 'Build/PackageFiles/ExtensionBundle', name), 'utf8')));
const temporary = await mkdtemp(path.join(os.tmpdir(), 'node-connector-templates-'));
const dependencyApp = process.env.NODE_CONNECTOR_TEST_DEPENDENCIES;
const generatedTypeScript = [];
const javascriptDirectory = path.join(temporary, 'javascript');

try {
    await mkdir(javascriptDirectory);
    await writeFile(path.join(javascriptDirectory, 'package.json'), JSON.stringify({ type: 'commonjs' }));
    if (dependencyApp) {
        await symlink(path.resolve(dependencyApp, 'node_modules'), path.join(temporary, 'node_modules'), process.platform === 'win32' ? 'junction' : 'dir');
    }

    await test('runtime dependencies provide Functions 4.17.1 enums and binding defaults', { skip: !dependencyApp }, () => {
        const require = createRequire(path.join(temporary, 'package.json'));
        const version = require('@azure/functions/package.json').version;
        const match = version.match(/^4\.(\d+)\.(\d+)(?:\+[\w.-]+)?$/);
        assert.ok(match, 'The template requires a stable Functions v4 release.');
        assert.ok(Number(match[1]) > 17 || (Number(match[1]) === 17 && Number(match[2]) >= 1), 'The template requires Functions 4.17.1 or later.');
        const { DeliveryMode, Cardinality, trigger } = require('@azure/functions');
        assert.deepEqual(DeliveryMode, { Webhook: 'Webhook', Poll: 'Poll' });
        assert.deepEqual(Cardinality, { One: 'one', Many: 'many' });
        const binding = trigger.connectorTrigger({});
        assert.equal(binding.deliveryMode, DeliveryMode.Webhook);
        assert.equal(binding.cardinality, Cardinality.One);
    });

    await test('both binding catalogs resolve the connector prompts and documentation', async () => {
        const bindings = catalogs.map((catalog) => catalog.bindings.filter((binding) => binding.type === 'connectorTrigger'));
        assert.ok(bindings.every((entries) => entries.length === 1));
        assert.deepEqual(bindings[0], bindings[1]);
        const binding = bindings[0][0];
        assert.equal(binding.direction, 'trigger');
        assert.equal(binding.enabledInTryMode, false);
        assert.equal(binding.documentation, '$content=Documentation\\connectorTrigger.md');
        await readFile(path.join(root, 'Functions.Templates/Documentation/connectorTrigger.md'), 'utf8');
        for (const reference of JSON.stringify(binding).matchAll(/\$([A-Za-z][A-Za-z0-9_]+)/g)) {
            if (reference[1] !== 'content') assert.ok(resources.has(reference[1]), reference[1]);
        }
        assert.deepEqual(binding.settings.map((setting) => setting.name), ['connection', 'triggerConfigName']);
        assert.ok(binding.settings.every((setting) => setting.required && setting.value === 'string'));
        assert.equal(binding.settings[0].defaultValue, 'ConnectorNamespace');
    });

    await test('public and official pipelines invoke JavaScript and TypeScript template checks with Node 24', async () => {
        const job = await readFile(path.join(root, 'eng/ci/templates/jobs/build.yml'), 'utf8');
        assert.match(job, /task: UseNode@1[\s\S]*?version: '24\.x'/);
        assert.ok(job.includes('node --test .\\Tools\\tests\\node-connector-templates.test.mjs'));
        for (const pipeline of ['public-build.yml', 'official-build.yml']) {
            const entry = await readFile(path.join(root, 'eng', pipeline), 'utf8');
            assert.ok(entry.includes('template: eng/ci/templates/jobs/build.yml@self'));
        }
    });

    await test('the guide explicitly limits JavaScript templates to CommonJS projects', async () => {
        const guide = await readFile(path.join(root, 'Functions.Templates/Documentation/connectorTrigger.md'), 'utf8');
        assert.match(guide, /CommonJS-only/);
        assert.ok(guide.includes('"type": "module"'));
        assert.match(guide, /not filtered by package module type/);
    });

    for (const connector of connectors) {
        const pythonSource = await readFile(path.join(root, 'Functions.Templates/Templates-v2', `${connector.name}Trigger-Python`, 'function_app.py'), 'utf8');
        const pythonOperations = [...pythonSource.matchAll(/^# (\w+) - list\[\w+\.\w+\]/gm)].map((match) => match[1]);
        let expectedPayloadDocumentation;
        await test(`${connector.name}: Python template remains separately packaged`, async () => {
            const id = `${connector.name}Trigger-Python`;
            const template = JSON.parse(await readFile(path.join(root, 'Functions.Templates/Templates-v2', id, 'template.json'), 'utf8'));
            assert.equal(template.programmingModel, 'v2');
            assert.equal(template.language, 'python');
            assert.ok(packages.every((nuspec) => nuspec.includes(`target="templates-v2\\${id}"`)));
        });

        for (const [language, extension] of Object.entries(languages)) {
            const id = `${connector.name}Trigger-${language}-4.x`;
            const directory = path.join(root, 'Functions.Templates/Templates', id);
            const metadata = JSON.parse(await readFile(path.join(directory, 'metadata.json'), 'utf8'));
            const fileName = `%functionName%.${extension}`;
            const source = await readFile(path.join(directory, fileName), 'utf8');
            const payloadDocumentation = [...source.matchAll(/^\/\/ (\w+) - (\w+)(?:\[\])?$/gm)]
                .map((match) => ({ operation: match[1], model: match[2] }));

            await test(`${id}: v4 metadata, resources, and both packages are complete`, async () => {
                assert.equal(metadata.language, language);
                assert.equal(metadata.name, connector.label);
                assert.equal(metadata.triggerType, 'connectorTrigger');
                assert.equal(metadata.enabledInTryMode, false);
                assert.deepEqual(metadata.userPrompt, ['connection', 'triggerConfigName']);
                assert.ok(resources.has(metadata.description.slice(1)));
                assert.match(metadata.defaultFunctionName, /^[a-zA-Z][a-zA-Z0-9_]{0,127}$/);
                assert.deepEqual((await readdir(directory)).sort(), [fileName, 'metadata.json'].sort());
                for (const nuspec of packages) {
                    assert.ok(nuspec.includes(`Templates\\${id}\\*.*" target="templates\\${id}"`), 'Retain the -4.x suffix for Node v4 discovery.');
                }
                assert.match(source, /deliveryMode: DeliveryMode\.Poll/);
                assert.doesNotMatch(source, /\bcardinality\s*:/, 'Single-event templates must rely on the library default.');
                assert.ok(source.includes(`triggerConfigName: '%${connector.setting}%'`));
                assert.doesNotMatch(source, /\bany\b|console\.log|function\.json|deliveryMode: 'Webhook'/);
                assert.ok(payloadDocumentation.length >= 2, 'List the selected and alternative operation payloads.');
                assert.ok(payloadDocumentation.some(({ operation }) => operation === connector.operation));
                if (expectedPayloadDocumentation) {
                    assert.deepEqual(payloadDocumentation, expectedPayloadDocumentation, 'Both languages must document the same operation/model mappings.');
                } else {
                    expectedPayloadDocumentation = payloadDocumentation;
                }
            });

            for (const name of [metadata.defaultFunctionName, `custom${connector.name}Poll`]) {
                const generated = source.replaceAll('%functionName%', name);
                const runtimeSource = language === 'TypeScript' ? stripTypeScriptTypes(generated) : generated;
                const generatedPath = language === 'TypeScript'
                    ? path.join(temporary, `${name}-${language}.mjs`)
                    : path.join(javascriptDirectory, `${name}.js`);
                await writeFile(generatedPath, runtimeSource);
                if (language === 'TypeScript') {
                    const typeScriptPath = path.join(temporary, `${name}.ts`);
                    await writeFile(typeScriptPath, generated);
                    generatedTypeScript.push(typeScriptPath);
                }

                await test(`${id}: generates valid code with function name ${name}`, () => {
                    assert.ok(!generated.includes('%functionName%'));
                    assert.ok(generated.includes(`'${name}'`));
                    const result = spawnSync(process.execPath, ['--check', generatedPath], { encoding: 'utf8' });
                    assert.ifError(result.error);
                    assert.equal(result.status, 0, result.stderr);
                });

                await test(`${id}: ${name} registers and normalizes real SDK payloads`, { skip: !dependencyApp }, async () => {
                    const require = createRequire(generatedPath);
                    const { app, trigger, DeliveryMode, Cardinality } = require('@azure/functions');
                    const sdk = require(`@azure/connectors/generated/${connector.sdk}Extensions`);
                    assert.equal(sdk[`${connector.sdk}TriggerOperations`][connector.key], connector.operation);
                    const operations = new Set(Object.values(sdk[`${connector.sdk}TriggerOperations`]));
                    const expectedOperations = new Set([connector.operation, ...pythonOperations.filter((operation) => operations.has(operation))]);
                    assert.deepEqual(new Set(payloadDocumentation.map(({ operation }) => operation)), expectedOperations, 'Match the Python operation context that this Node SDK actually exports.');
                    const declarations = await readFile(require.resolve(`@azure/connectors/generated/${connector.sdk}Extensions`).replace(/\.js$/, '.d.ts'), 'utf8');
                    for (const { operation, model } of payloadDocumentation) {
                        assert.ok(operations.has(operation), `Missing SDK operation ${operation}.`);
                        assert.match(declarations, new RegExp(`export (?:interface|type) ${model}\\b`), `Missing SDK payload model ${model}.`);
                    }
                    const original = app.connectorTrigger;
                    const registrations = [];
                    app.connectorTrigger = (registeredName, options) => registrations.push({ registeredName, options });
                    try {
                        await import(pathToFileURL(generatedPath).href);
                    } finally {
                        app.connectorTrigger = original;
                    }
                    assert.equal(registrations.length, 1);
                    const { registeredName, options: { handler, ...options } } = registrations[0];
                    assert.equal(registeredName, name);
                    assert.deepEqual(options, {
                        deliveryMode: DeliveryMode.Poll,
                        connection: 'ConnectorNamespace',
                        triggerConfigName: `%${connector.setting}%`,
                    });
                    const binding = trigger.connectorTrigger(options);
                    assert.equal(binding.deliveryMode, DeliveryMode.Poll);
                    assert.equal(binding.cardinality, Cardinality.One);
                    const item = { secretField: 'do-not-log-this-payload' };
                    const payloads = [
                        [{ body: item }, 1],
                        [{ body: { value: [item, item] } }, 2],
                        [[{ body: item }, { body: { value: [item] } }], 2],
                        [JSON.stringify([{ body: item }, { body: item }]), 2],
                        [[], 0],
                        [null, 0],
                    ];
                    for (const [payload, count] of payloads) {
                        const logs = [];
                        await handler(payload, {
                            log: (...args) => logs.push(args.join(' ')),
                            warn: (...args) => assert.fail(args.join(' ')),
                        });
                        assert.equal(logs.length, 1);
                        assert.ok(logs[0].includes(`Received ${count} `));
                        assert.ok(!logs[0].includes(item.secretField));
                    }
                });
            }
        }
    }

    await test('all generated TypeScript modules pass strict SDK type checking', { skip: !dependencyApp }, () => {
        const require = createRequire(path.join(temporary, 'package.json'));
        const compiler = require.resolve('typescript/bin/tsc');
        const result = spawnSync(process.execPath, [
            compiler, '--noEmit', '--strict', '--module', 'Node16',
            '--moduleResolution', 'Node16', '--target', 'ES2022',
            ...generatedTypeScript,
        ], { encoding: 'utf8', cwd: temporary });
        assert.ifError(result.error);
        assert.equal(result.status, 0, result.stdout + result.stderr);
    });
} finally {
    await rm(temporary, { recursive: true, force: true });
}
