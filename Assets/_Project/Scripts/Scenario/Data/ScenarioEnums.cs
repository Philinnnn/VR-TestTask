namespace _Project.Scripts.Scenario.Data
{
    public enum ActionType
    {
        ZoneEnter,
        GrabObject,
        ClickObject,
        ClickUIButton
    }

    public enum StepStatus
    {
        Pending,
        Success,
        Failed,
        Skipped 
    }
}