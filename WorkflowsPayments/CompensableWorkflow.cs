using Elsa;
using Elsa.Activities.Compensation;
using Elsa.Activities.Primitives;
using Elsa.ActivityResults;
using Elsa.Builders;
using Elsa.Services;
using Elsa.Services.Models;

namespace WorkflowsPayments
{
    /// <summary>
    /// https://github.com/elsa-workflows/elsa-core/issues/2683
    /// </summary>
    public class CompensableWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .WithWorkflowDefinitionId(nameof(CompensableWorkflow))
                .StartWith<Compensable>(compensable =>
                {
                    compensable.When(OutcomeNames.Body)
                        .Then<ChargeCreditCard>()
                        .Then<ReserveFlight>();

                    compensable.When(OutcomeNames.Compensate)
                        .Then<CancelFlight>()
                        .Then<CancelCreditCardCharges>();

                    compensable.When(OutcomeNames.Confirm)
                        .Then<ConfirmFlight>();

                }).WithName("Compensable1")
                .Then<ManagerApproval>()
                .Then<Fault>(a => a.WithMessage("Critical system error!"))
                .Then<PurchaseFlight>()
                .Then<TakeFlight>()
                .Then<Confirm>(a => a.WithCompensableActivityName("Compensable1"));
        }
    }

    public class TakeFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Flight has been taken, no compensation possible");
            return Done();
        }
    }

    public class PurchaseFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Ticket is purchased");
            return Done();
        }
    }

    public class ManagerApproval    : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Manager approval received");
            return Done();
        }
    }

    public class ConfirmFlight  : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Confirming flight");
            return Done();
        }
    }

    public class CancelCreditCardCharges    : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Cancelling credit card charges");
            return Done();
        }
    }

    public class CancelFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Cancelling flight");
            return Done();
        }
    }

    public class ReserveFlight  : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Reserving flight");
            return Done();
        }
    }

    public class ChargeCreditCard : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine("Charging credit card for flight");
            return Done();
        }
    }
}
