using DocumentManagement.Workflows.Activities;
using Elsa;
using Elsa.Activities.Compensation;
using Elsa.ActivityResults;
using Elsa.Builders;
using Elsa.Services;

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
                    /*
                     * The Body : what work to be done within the scope of the compensable activity.
                     */
                    compensable.When(OutcomeNames.Body)
                        .Then<ChooseFlight>()
                        .Then<ChargeCreditCard>()
                        .Then<UpdateBlockchain>()
                        .Then<ReserveFlight>();

                    /*
                     *If any activity causes an unhandled exception in the Done branch,
                     * the Compensate outcome of the compensable activity will be scheduled,
                     * allowing the workflow to undo actions as necessary.
                     */
                    compensable.When(OutcomeNames.Compensate)
                        .Then<CancelFlight>()
                        .Then<CancelCreditCardCharges>();

                    /*
                     * For some scenarios, compensable activities should no longer allow to be compensated anymore.
                     * To control this, the user should be able to explicitly confirm a compensable activity.
                     * When this happens, the Confirm outcome will be scheduled, allowing the user to do any work that finalizes some state.
                     */
                    compensable.When(OutcomeNames.Confirm)
                        .Then<ConfirmFlight>();

                    // Once the Body branch completes, the Done outcome is scheduled.
                    compensable.When(OutcomeNames.Done)
                        .Then<ReviewFlight>();

                    // If an activity within the Body branch faults, the Cancel outcome is scheduled.
                    compensable.When(OutcomeNames.Cancel)
                        .Then<CancelFlight>();

                }).WithName("Compensable1")
                .Then<ManagerApproval>()
                //.Then<Fault>(a => a.WithMessage("******* Critical system error!"))
                .Then<PurchaseFlight>()
                .Then<TakeFlight>()
                .Then<Confirm>(a => a.WithCompensableActivityName("Compensable1"));
        }
    }

    public class TakeFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Flight has been taken, no compensation possible");
            return Done();
        }
    }

    public class ChooseFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Flight Chosen");
            return Done();
        }
    }

    public class ReviewFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Reviewed flight. Thank you for flying with us! ");
            return Done();
        }
    }

    public class PurchaseFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Ticket is purchased");
            return Done();
        }
    }

    public class ManagerApproval    : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Manager approval received");
            return Done();
        }
    }

    public class ConfirmFlight  : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Confirming flight");
            return Done();
        }
    }

    public class CancelCreditCardCharges    : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Cancelling credit card charges");
            return Done();
        }
    }

    public class CancelFlight : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Cancelling flight");
            return Done();
        }
    }

    public class ReserveFlight  : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Reserving flight");

            return Fault(new OperationCanceledException("Flight Cancelled"));
            //return Done();
        }
    }

    public class ChargeCreditCard : Activity
    {
        protected override IActivityExecutionResult OnExecute()
        {
            Console.WriteLine(">>>>>>>  Charging credit card for flight");
            return Done();
        }
    }
}
