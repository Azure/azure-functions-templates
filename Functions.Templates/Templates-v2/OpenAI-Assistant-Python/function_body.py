todo_manager = CreateTodoManager()

@app.function_name("CreateAssistant")
@app.route(route="assistants/{assistantId}", methods=["PUT"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@app.assistant_create_output(arg_name="requests")
def create_assistant(req: func.HttpRequest, requests: func.Out[str]) -> func.HttpResponse:
    assistantId = req.route_params.get("assistantId")
    instructions = """
            Don't make assumptions about what values to plug into functions.
            Ask for clarification if a user request is ambiguous.
            """
    create_request = {
        "id": assistantId,
        "instructions": instructions
    }
    requests.set(json.dumps(create_request))
    response_json = {"assistantId": assistantId}
    return func.HttpResponse(json.dumps(response_json), status_code=202, mimetype="application/json")


@app.function_name("PostUserQuery")
@app.route(route="assistants/{assistantId}", methods=["POST"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@app.assistant_post_input(arg_name="state", id="{assistantId}", user_message="{Query.message}", model="$(CHAT_MODEL_NAME)")
def post_user_response(req: func.HttpRequest, state: str) -> func.HttpResponse:
    # Parse the JSON string into a dictionary
    data = json.loads(state)

    # Extract the content of the recentMessage
    recent_message_content = data['recentMessages'][0]['content']
    return func.HttpResponse(recent_message_content, status_code=200, mimetype="text/plain")


@app.function_name("GetChatState")
@app.route(route="assistants/{assistantId}", methods=["GET"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@app.assistant_query_input(arg_name="state", id="{assistantId}", timestamp_utc="{Query.timestampUTC}")
def get_chat_state(req: func.HttpRequest, state: str) -> func.HttpResponse:
    return func.HttpResponse(state, status_code=200, mimetype="application/json")


@app.function_name("AddTodo")
@app.assistant_skill_trigger(arg_name="taskDescription", function_description="Create a new todo task")
def add_todo(taskDescription: str) -> None:
    if not taskDescription:
        raise ValueError("Task description cannot be empty")

    logging.info(f"Adding todo: {taskDescription}")

    todo_id = str(uuid.uuid4())[0:6]
    todo_manager.add_todo(TodoItem(id=todo_id, task=taskDescription))
    return


@app.function_name("GetTodos")
@app.assistant_skill_trigger(arg_name="inputIgnored", function_description="Fetch the list of previously created todo tasks")
def get_todos(inputIgnored: str) -> str:
    logging.info("Fetching list of todos")
    results = todo_manager.get_todos()
    return json.dumps(results)

from azure.cosmos import CosmosClient
from azure.cosmos import PartitionKey

class TodoItem:
    def __init__(self, id, task):
        self.id = id
        self.task = task


class ITodoManager(metaclass=abc.ABCMeta):
    @abc.abstractmethod
    def add_todo(self, todo: TodoItem):
        raise NotImplementedError()

    @abc.abstractmethod
    def get_todos(self):
        raise NotImplementedError()


class InMemoryTodoManager(ITodoManager):
    def __init__(self):
        self.todos = []

    def add_todo(self, todo: TodoItem):
        self.todos.append(todo)

    def get_todos(self):
        return [item.__dict__ for item in self.todos]


class CosmosDbTodoManager(ITodoManager):
    def __init__(self, cosmos_client: CosmosClient):
        self.cosmos_client = cosmos_client
        cosmos_database_name = os.environ.get("CosmosDatabaseName")
        cosmos_container_name = os.environ.get("CosmosContainerName")

        if not cosmos_database_name or not cosmos_container_name:
            raise ValueError("CosmosDatabaseName and CosmosContainerName must be set as environment variables or in local.settings.json")
        
        self.database = self.cosmos_client.create_database_if_not_exists(cosmos_database_name)
        self.container = self.database.create_container_if_not_exists(id=cosmos_container_name, partition_key=PartitionKey(path="/id"))

    def add_todo(self, todo: TodoItem):
        logging.info(
            f"Adding todo ID = {todo.id} to container '{self.container.id}'.")
        self.container.create_item(todo.__dict__)

    def get_todos(self):
        logging.info(
            f"Getting all todos from container '{self.container.id}'.")
        results = [item for item in self.container.query_items(
            "SELECT * FROM c", enable_cross_partition_query=True)]
        logging.info(
            f"Found {len(results)} todos in container '{self.container.id}'.")
        return results


def CreateTodoManager() -> ITodoManager:
    if not os.environ.get("CosmosDbConnectionString"):
        return InMemoryTodoManager()
    else:
        cosmos_client = CosmosClient.from_connection_string(
            os.environ["CosmosDbConnectionString"])
        return CosmosDbTodoManager(cosmos_client)