using _Project.Scripts.Interactions;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.Scenario
{
    public enum ValidationResultType
    {
        Success,
        Failed,
        SequenceViolation  // Invalid sequence
    }

    /// <summary>
    /// Validates the interaction against the current step and step group.
    /// </summary>
    public class StepValidator
    {
        public ValidationResultType Validate(
            IInteractable interactable, 
            StepSO currentStep, 
            StepGroupSO currentGroup, 
            int currentStepIndex)
        {
            if (interactable == null || currentStep == null || currentGroup == null)
            {
                return ValidationResultType.Failed;
            }
            
            // Future sequence check
            for (var i = currentStepIndex + 1; i < currentGroup.Steps.Count; i++)
            {
                var futureStep = currentGroup.Steps[i];
                if (futureStep != null && 
                    futureStep.ExpectedAction == interactable.ActionType && 
                    futureStep.TargetId == interactable.TargetId)
                {
                    return ValidationResultType.SequenceViolation;
                }
            }
            
            // Checking the current expected step
            if (currentStep.ExpectedAction == interactable.ActionType && 
                currentStep.TargetId == interactable.TargetId)
            {
                return ValidationResultType.Success;
            }

            // Invalid action or target
            return ValidationResultType.Failed;
        }
    }
}