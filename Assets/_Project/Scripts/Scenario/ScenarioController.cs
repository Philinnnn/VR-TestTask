using System;
using UnityEngine;
using _Project.Scripts.Interactions;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.Scenario
{
    [RequireComponent(typeof(InteractionRegistrar))]
    public class ScenarioController : MonoBehaviour
    {
        [SerializeField] private ScenarioSO scenarioData;

        private InteractionRegistrar _registrar;
        private StepValidator _validator;

        private int _currentGroupIndex;
        private int _currentStepIndex;
        private bool _isScenarioActive;

        public event Action<StepSO> OnStepStarted;
        public event Action<StepSO, StepStatus> OnStepCompleted;
        public event Action<StepGroupSO> OnGroupStarted;
        public event Action<StepGroupSO> OnGroupCompleted;
        public event Action OnSequenceError;
        public event Action OnScenarioFinished;

        public StepSO CurrentStep => GetCurrentStep();
        public StepGroupSO CurrentGroup => GetCurrentGroup();
        public bool IsActive => _isScenarioActive;

        private void Awake()
        {
            _registrar = GetComponent<InteractionRegistrar>();
            _validator = new StepValidator();
        }

        private void OnEnable()
        {
            _registrar.OnAnyInteraction += ProcessInteraction;
        }

        private void OnDisable()
        {
            _registrar.OnAnyInteraction -= ProcessInteraction;
        }

        private void Start()
        {
            if (scenarioData != null)
            {
                StartScenario();
            }
        }

        public void StartScenario()
        {
            _currentGroupIndex = 0;
            _currentStepIndex = 0;
            _isScenarioActive = true;
            StartCurrentGroup();
        }

        private void StartCurrentGroup()
        {
            if (scenarioData == null || _currentGroupIndex >= scenarioData.StepGroups.Count)
            {
                FinishScenario();
                return;
            }

            _currentStepIndex = 0;
            OnGroupStarted?.Invoke(GetCurrentGroup());
            StartCurrentStep();
        }

        private void StartCurrentStep()
        {
            var step = GetCurrentStep();
            if (step != null)
            {
                OnStepStarted?.Invoke(step);
            }
        }

        private void ProcessInteraction(IInteractable interactable)
        {
            if (!_isScenarioActive) return;

            var activeStep = GetCurrentStep();
            var activeGroup = GetCurrentGroup();

            var result = _validator.Validate(interactable, activeStep, activeGroup, _currentStepIndex);

            switch (result)
            {
                case ValidationResultType.Success:
                    OnStepCompleted?.Invoke(activeStep, StepStatus.Success);
                    AdvanceToNextStep();
                    break;

                case ValidationResultType.Failed:
                    OnStepCompleted?.Invoke(activeStep, StepStatus.Failed);
                    AdvanceToNextStep();
                    break;

                case ValidationResultType.SequenceViolation:
                    OnSequenceError?.Invoke();
                    OnGroupCompleted?.Invoke(activeGroup);
                    AdvanceToNextGroup();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AdvanceToNextStep()
        {
            _currentStepIndex++;
            var group = GetCurrentGroup();

            if (group == null || _currentStepIndex >= group.Steps.Count)
            {
                if (group != null)
                {
                    OnGroupCompleted?.Invoke(group);
                }
                AdvanceToNextGroup();
            }
            else
            {
                StartCurrentStep();
            }
        }

        private void AdvanceToNextGroup()
        {
            _currentGroupIndex++;
            StartCurrentGroup();
        }

        private void FinishScenario()
        {
            _isScenarioActive = false;
            OnScenarioFinished?.Invoke();
        }

        private StepGroupSO GetCurrentGroup()
        {
            if (scenarioData != null && _currentGroupIndex >= 0 && _currentGroupIndex < scenarioData.StepGroups.Count)
                return scenarioData.StepGroups[_currentGroupIndex];
            return null;
        }

        private StepSO GetCurrentStep()
        {
            var group = GetCurrentGroup();
            if (group != null && _currentStepIndex >= 0 && _currentStepIndex < group.Steps.Count)
                return group.Steps[_currentStepIndex];
            return null;
        }
    }
}