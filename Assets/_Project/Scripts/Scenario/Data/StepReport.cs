namespace _Project.Scripts.Scenario.Data
{
    /// <summary>
    /// Represents a report of a step's execution status in the scenario.
    /// </summary>
    public struct StepReport
    {
        public readonly StepSO Step;
        public readonly StepStatus Status;

        public StepReport(StepSO step, StepStatus status)
        {
            Step = step;
            Status = status;
        }
    }
}