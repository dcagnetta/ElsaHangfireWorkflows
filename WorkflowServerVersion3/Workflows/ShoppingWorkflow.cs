using Elsa.Workflows.Core.Abstractions;
using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Contracts;
using WorkflowServerVersion3.Activities;

namespace WorkflowServerVersion3.Workflows;

public class ShoppingWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder workflow)
    {
        workflow.DefinitionId = nameof(ShoppingWorkflow);
        workflow.Root = new Sequence
        {
            Activities =
            {
                new AddItemToCart()
            }
        };
    }
}