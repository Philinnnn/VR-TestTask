using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Interactions;
using _Project.Scripts.Interactions.Highlighting;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.Scenario
{
    /// <summary>
    /// Highlights whatever the scenario currently expects the user to
    /// interact with, and clears the highlight once that target's action
    /// is done
    /// </summary>
    [RequireComponent(typeof(InteractionRegistrar))]
    public class ScenarioHighlighter : MonoBehaviour
    {
        [SerializeField] private ScenarioController scenarioController;

        private InteractionRegistrar _registrar;

        private void Awake()
        {
            _registrar = GetComponent<InteractionRegistrar>();
        }

        private void OnEnable()
        {
            if (scenarioController == null) return;

            scenarioController.OnStepStarted += HighlightStep;
            scenarioController.OnStepActionCompleted += HandleActionCompleted;
            scenarioController.OnStepCompleted += HandleStepCompleted;
            scenarioController.OnGroupCompleted += HandleGroupCompleted;
            scenarioController.OnScenarioFinished += HandleScenarioFinished;
            
            StartCoroutine(RefreshHighlightNextFrame());
        }

        private IEnumerator RefreshHighlightNextFrame()
        {
            yield return null;

            if (scenarioController != null && scenarioController.IsActive && scenarioController.CurrentStep != null)
            {
                ClearAllHighlights();
                HighlightStep(scenarioController.CurrentStep);
            }
        }

        private void OnDisable()
        {
            if (scenarioController == null) return;

            scenarioController.OnStepStarted -= HighlightStep;
            scenarioController.OnStepActionCompleted -= HandleActionCompleted;
            scenarioController.OnStepCompleted -= HandleStepCompleted;
            scenarioController.OnGroupCompleted -= HandleGroupCompleted;
            scenarioController.OnScenarioFinished -= HandleScenarioFinished;
        }

        private void HighlightStep(StepSO step)
        {
            if (step == null) return;

            foreach (var action in step.ExpectedActions)
            {
                SetHighlightForTarget(action.ActionType, action.TargetId, true);
            }
        }

        private void HandleActionCompleted(StepSO step, StepAction action)
        {
            SetHighlightForTarget(action.ActionType, action.TargetId, false);
        }

        private void HandleStepCompleted(StepSO step, StepStatus status)
        {
            if (step == null) return;

            foreach (var action in step.ExpectedActions)
            {
                SetHighlightForTarget(action.ActionType, action.TargetId, false);
            }
        }

        private void HandleGroupCompleted(StepGroupSO group)
        {
            ClearAllHighlights();
        }

        private void HandleScenarioFinished(IReadOnlyList<StepReport> reports)
        {
            ClearAllHighlights();
        }

        private void SetHighlightForTarget(ActionType actionType, string targetId, bool on)
        {
            if (_registrar == null) return;

            foreach (var interactable in _registrar.RegisteredInteractables)
            {
                if (interactable.ActionType != actionType || interactable.TargetId != targetId) continue;

                if (interactable is Component component && component.TryGetComponent(out IHighlightable highlightable))
                {
                    highlightable.SetHighlighted(on);
                }
            }
        }

        private void ClearAllHighlights()
        {
            if (_registrar == null) return;

            foreach (var interactable in _registrar.RegisteredInteractables)
            {
                if (interactable is Component component && component.TryGetComponent(out IHighlightable highlightable))
                {
                    highlightable.SetHighlighted(false);
                }
            }
        }
    }
}