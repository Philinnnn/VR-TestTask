using System.Collections.Generic;
using _Project.Scripts.Interactions;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.Scenario
{
    public enum ValidationResultType
    {
        ActionSuccess,      // one of the step's required actions was performed, but the step still has actions left
        StepSuccess,        // the performed action was the last one needed to finish the step
        Failed,             // action/target that doesn't belong to the current step
        SequenceViolation   // action belongs to a future step -> order was broken
    }

    public readonly struct ValidationResult
    {
        public readonly ValidationResultType Type;
        public readonly int MatchedActionIndex; 

        public ValidationResult(ValidationResultType type, int matchedActionIndex = -1)
        {
            Type = type;
            MatchedActionIndex = matchedActionIndex;
        }
    }

    /// <summary>
    /// Validates an interaction against the current step and step group.
    /// A step may require several actions (in any order); the caller is
    /// responsible for tracking which of them are already completed and
    /// passing that in via <paramref name="completedActionIndices"/>.
    /// </summary>
    public class StepValidator
    {
        public ValidationResult Validate(
            IInteractable interactable,
            StepSO currentStep,
            StepGroupSO currentGroup,
            int currentStepIndex,
            ISet<int> completedActionIndices)
        {
            if (interactable == null || currentStep == null || currentGroup == null)
            {
                return new ValidationResult(ValidationResultType.Failed);
            }

            // Future sequence check: an action that matches something from a
            // later step means the user jumped ahead in the sequence.
            for (var i = currentStepIndex + 1; i < currentGroup.Steps.Count; i++)
            {
                var futureStep = currentGroup.Steps[i];
                if (futureStep == null) continue;

                foreach (var action in futureStep.ExpectedActions)
                {
                    if (Matches(action, interactable))
                    {
                        return new ValidationResult(ValidationResultType.SequenceViolation);
                    }
                }
            }
            
            var actions = currentStep.ExpectedActions;
            for (var i = 0; i < actions.Count; i++)
            {
                if (completedActionIndices.Contains(i)) continue;

                if (Matches(actions[i], interactable))
                {
                    var isLastRemaining = completedActionIndices.Count + 1 >= actions.Count;
                    return new ValidationResult(
                        isLastRemaining ? ValidationResultType.StepSuccess : ValidationResultType.ActionSuccess,
                        i);
                }
            }
            
            return new ValidationResult(ValidationResultType.Failed);
        }

        private static bool Matches(StepAction action, IInteractable interactable)
        {
            return action != null &&
                   action.ActionType == interactable.ActionType &&
                   action.TargetId == interactable.TargetId;
        }
    }
}