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
                    Trace.WriteLine("Starting the very slow workflow");

                })
                .Then(async x =>
                {
                    Trace.WriteLine("Delaying for 5 seconds...");
                    await Task.Delay(new TimeSpan(0, 0, 5));
                    Trace.WriteLine("Done with delay!");
                })
                .Then(async x =>
                {
                    Trace.WriteLine("Delaying for 5 seconds...");
                    await Task.Delay(new TimeSpan(0, 0, 5));
                    Trace.WriteLine("Done with delay!");
                })
                .Then(x =>
                {
                    Trace.WriteLine("All done!");
                });
            ;
        }
    }
}
