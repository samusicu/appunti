using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;

public class IsolatedExample
{
    [Function("HttpTriggerIsolated")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext context)
    {
        var logger = context.GetLogger("HttpTriggerIsolated");
        logger.LogInformation("C# HTTP trigger function processed a request.");
        
        // Accesso ai parametri query diverso
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string name = query["name"] ?? "";
        
        // Creazione response esplicita
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"Hello, {name}");
        
        return response;
    }
    
    // Durable Functions Isolated
    [Function("DurableOrchestratorIsolated")]
    public async Task<string> RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger<IsolatedExample>();
        var input = context.GetInput<string>();
        var result = await context.CallActivityAsync<string>("ActivityFunction", input);
        return result;
    }
}