using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public static class InProcessExample
{
    [FunctionName("HttpTriggerInProcess")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        
        // Accesso diretto agli oggetti ASP.NET Core
        string name = req.Query["name"];
        
        return new OkObjectResult($"Hello, {name}");
    }
    
    // Durable Functions In-Process
    [FunctionName("DurableOrchestratorInProcess")]
    public static async Task<string> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var input = context.GetInput<string>();
        var result = await context.CallActivityAsync<string>("ActivityFunction", input);
        return result;
    }
}
