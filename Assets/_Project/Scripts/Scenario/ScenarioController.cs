using System;
using System.Collections.Generic;
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
        
        private readonly HashSet<int> _completedActionIndices = new HashSet<int>();

        private readonly List<StepReport> _reports = new List<StepReport>();

        public event Action<StepSO> OnStepStarted;
        public event Action<StepSO, StepAction> OnStepActionCompleted;
        public event Action<StepSO, StepStatus> OnStepCompleted;
        public event Action<StepGroupSO> OnGroupStarted;
        public event Action<StepGroupSO> OnGroupCompleted;
        public event Action OnSequenceError;
        public event Action<IReadOnlyList<StepReport>> OnScenarioFinished;

        public StepSO CurrentStep => GetCurrentStep();
        public StepGroupSO CurrentGroup => GetCurrentGroup();
        public bool IsActive { get; private set; }

        public IReadOnlyList<StepReport> Reports => _reports;

        private void Awake()
        {
            _registrar = GetComponent<InteractionRegistrar>();
            _validator = new StepValidator();
        }

        private void OnEnable()
        {
            if (_registrar != null)
                _registrar.OnAnyInteraction += ProcessInteraction;
        }

        private void OnDisable()
        {
            if (_registrar != null)
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
            _reports.Clear();
            _currentGroupIndex = 0;
            _currentStepIndex = 0;
            IsActive = true;
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
            _completedActionIndices.Clear();

            var step = GetCurrentStep();
            if (step != null)
            {
                OnStepStarted?.Invoke(step);
            }
        }

        private void ProcessInteraction(IInteractable interactable)
        {
            if (!IsActive) return;

            var activeStep = GetCurrentStep();
            var activeGroup = GetCurrentGroup();

            var result = _validator.Validate(
                interactable, activeStep, activeGroup, _currentStepIndex, _completedActionIndices);

            switch (result.Type)
            {
                case ValidationResultType.ActionSuccess:
                    _completedActionIndices.Add(result.MatchedActionIndex);
                    OnStepActionCompleted?.Invoke(activeStep, activeStep.ExpectedActions[result.MatchedActionIndex]);
                    break;

                case ValidationResultType.StepSuccess:
                    _reports.Add(new StepReport(activeStep, StepStatus.Success));
                    OnStepCompleted?.Invoke(activeStep, StepStatus.Success);
                    AdvanceToNextStep();
                    break;

                case ValidationResultType.Failed:
                    _reports.Add(new StepReport(activeStep, StepStatus.Failed));
                    OnStepCompleted?.Invoke(activeStep, StepStatus.Failed);
                    AdvanceToNextStep();
                    break;

                case ValidationResultType.SequenceViolation:
                    _reports.Add(new StepReport(activeStep, StepStatus.Failed));

                    for (var i = _currentStepIndex + 1; i < activeGroup.Steps.Count; i++)
                    {
                        _reports.Add(new StepReport(activeGroup.Steps[i], StepStatus.Skipped));
                    }

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
            IsActive = false;
            OnScenarioFinished?.Invoke(_reports);
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