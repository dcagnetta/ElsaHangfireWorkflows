using Elsa.Workflows.Core.Models;

namespace WorkflowServerVersion3.Activities
{
    public class AddItemToCart : Activity
    {
        private readonly ILogger<AddItemToCart> _logger;

        /*public AddItemToCart(ILogger<AddItemToCart> logger)
        {
            _logger = logger;
        }*/
        protected override ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            //_logger.LogInformation("Step 1: Add Item To Cart");

            Console.Out.WriteLineAsync("Step 1: Add Item To Cart");

            Task.Delay(1000);

            return ValueTask.CompletedTask;
        }
    }
}
