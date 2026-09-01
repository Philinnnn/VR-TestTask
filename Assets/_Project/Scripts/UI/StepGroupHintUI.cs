using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Scenario;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Shows an informational panel listing the expected steps for a step
    /// group as soon as it starts, so the user knows the order of actions
    /// ahead of time.
    /// </summary>
    public class StepGroupHintUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScenarioController scenarioController;
        [SerializeField] private GameObject hintPanel;
        [SerializeField] private TMP_Text groupNameText;
        [SerializeField] private TMP_Text stepsListText;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (hintPanel != null)
                hintPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (scenarioController != null)
            {
                scenarioController.OnGroupStarted += ShowHint;
                scenarioController.OnGroupCompleted += HandleGroupCompleted;
                scenarioController.OnScenarioFinished += HandleScenarioFinished;
            }

            if (closeButton != null)
                closeButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            if (scenarioController != null)
            {
                scenarioController.OnGroupStarted -= ShowHint;
                scenarioController.OnGroupCompleted -= HandleGroupCompleted;
                scenarioController.OnScenarioFinished -= HandleScenarioFinished;
            }

            if (closeButton != null)
                closeButton.onClick.RemoveListener(Hide);
        }

        private void ShowHint(StepGroupSO group)
        {
            if (group == null || hintPanel == null) return;

            if (groupNameText != null)
                groupNameText.text = group.GroupName;

            if (stepsListText != null)
                stepsListText.text = BuildStepsList(group);

            hintPanel.SetActive(true);
        }
        
        private void HandleGroupCompleted(StepGroupSO group) => Hide();
        private void HandleScenarioFinished(IReadOnlyList<StepReport> reports) => Hide();

        private void Hide()
        {
            if (hintPanel != null)
                hintPanel.SetActive(false);
        }

        private static string BuildStepsList(StepGroupSO group)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < group.Steps.Count; i++)
            {
                var step = group.Steps[i];
                var description = step != null ? step.Description : "???";
                sb.AppendLine($"{i + 1}. {description}");
            }
            return sb.ToString();
        }
    }
}