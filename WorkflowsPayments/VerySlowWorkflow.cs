using System.Diagnostics;
using Elsa.Builders;

namespace WorkflowsPayments
{
    public class VerySlowWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .WithWorkflowDefinitionId(nameof(VerySlowWorkflow))
                .WithVersion(1)
                .StartWith(x =>
                {
                    Console.WriteLine("Step 1: Starting the very slow workflow");
                })
                .Then(async x =>
                {
                    Console.WriteLine("Step 2: Delaying for 5 seconds...");
                    await Task.Delay(new TimeSpan(0, 0, 5));
                    Console.WriteLine("Done with delay!");
                })
                .Then(async x =>
                {
                    Console.WriteLine("Step 3: Delaying for 5 seconds...");
                    await Task.Delay(new TimeSpan(0, 0, 5));
                    Console.WriteLine("Done with delay!");
                })
                .Then(x =>
                {
                    Console.WriteLine("Step 4: All done!");
                });
            ;
        }
    }
}
