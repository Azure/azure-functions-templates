Param(   
   [Parameter(Mandatory=$True)][string]$appName,  
   [Parameter(Mandatory=$True)][string]$password,
   [Parameter(Mandatory=$True)][string]$storageAccount,
   [Parameter(Mandatory=$True)][string]$storageKey,
   [Parameter(Mandatory=$True)][string]$templatesFolderPath,   
   [Parameter(Mandatory=$False)][bool]$appVeyorOutput,
   [Parameter(Mandatory=$False)][bool]$htmlOutput
)

# Load all the functions
. ./Test/lib.ps1
. ./Test/Tests.ps1

# run initializtion scripts
# 1 to make all the function available
function Init()
{    
    try 
    {  
        Write-Host -ForegroundColor Yellow "Initializing Tests"        

        $config = GetConfiguration $appName $password $storageAccount $storageKey        

        # Get the secret key from the function            
        $secretJson = GetFunctionSecret $config
        $config.Add("functionSecret",$secretJson)        
        $templates = GetTemplates $templatesFolderPath

        Write-Host -ForegroundColor Green "Test Initialization complete"  
        return @{ 
            config = $config  
            templates = $templates 
        }
    }
    catch
    {
        Write-Host -ForegroundColor Red "Initialization failed"    
        Write-Host $_.Exception.GetType().FullName, $_.Exception.Message
        throw $_.Exception
    }        
}

# This will be called before each test, would return a value that will be passed to each test
Function TestInit($config , $templates, $testName)
{
    AddHostFeed
} 

# Find and start executing test 
Function ExecuteTest()
{
    $aggregateTestResults = New-Object System.Collections.ArrayList

    # Execute 
    $initData = Init
    $tests = GetTestList    
    $config = $initData.config
    $templates = $initData.templates

    foreach($test in $tests)
    {
        $testName = $test.Name

        # passing the data to test init for pre configuration step         
        TestInit $initData.config $initData.templates $test.Name

        # execute the test
        $testResult = & $testName $config $templates

        TestCleanUp $config $templates $testName $testResult
        
        $index = $aggregateTestResults.Add($testResult)
    }

    CleanUp $config $templates $aggregateTestResults
}

# Will be executed after each test is complete
# if the test returns anything this function will be passed that value
Function TestCleanUp($config , $templates, $testName, $testResult)
{
    AddHostFeed $false
} 

# Called after  test execution for all the tests are complete
function CleanUp($config, $templates,$aggregateTestResults)
{    
    if ($appVeyorOutput)
    {        
        CreateAppveyorTests $aggregateTestResults
    }

    if ($htmlOutput)
    {
        #CreateHtmlOutPut $aggregateTestResults
    }
}    

ExecuteTest