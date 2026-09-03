using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using _Project.Scripts.Scenario;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.UI
{
    public class ScenarioResultUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScenarioController scenarioController;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Transform container;
        [SerializeField] private ScenarioResultItemUI itemPrefab;
        [SerializeField] private TMP_Text summaryText;

        [Header("Buttons")]
        [SerializeField] private Button restartButton;
        [SerializeField] private Button lobbyButton;

        private void Awake()
        {
            if (resultPanel != null)
                resultPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (scenarioController != null)
                scenarioController.OnScenarioFinished += ShowResults;

            if (restartButton != null)
                restartButton.onClick.AddListener(OnRestartClicked);

            if (lobbyButton != null)
                lobbyButton.onClick.AddListener(OnLobbyClicked);
        }

        private void OnDisable()
        {
            if (scenarioController != null)
                scenarioController.OnScenarioFinished -= ShowResults;

            if (restartButton != null)
                restartButton.onClick.RemoveListener(OnRestartClicked);

            if (lobbyButton != null)
                lobbyButton.onClick.RemoveListener(OnLobbyClicked);
        }

        private void ShowResults(IReadOnlyList<StepReport> reports)
        {
            if (resultPanel != null)
                resultPanel.SetActive(true);

            if (container == null || itemPrefab == null)
                return;

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            if (reports != null)
            {
                var successCount = 0;
                var failedCount = 0;
                var skippedCount = 0;

                for (var i = 0; i < reports.Count; i++)
                {
                    var report = reports[i];
                    var item = Instantiate(itemPrefab, container);
                    var description = report.Step != null ? report.Step.Description : "Unknown step";
                    item.Setup(i, description, report.Status);

                    switch (report.Status)
                    {
                        case StepStatus.Success:
                            successCount++;
                            break;
                        case StepStatus.Failed:
                            failedCount++;
                            break;
                        case StepStatus.Skipped:
                            skippedCount++;
                            break;
                    }
                }

                if (summaryText != null)
                {
                    summaryText.text = $"Result: total {reports.Count} | " +
                                       $"success {successCount} | " +
                                       $"failed {failedCount} | " +
                                       $"skipped {skippedCount}";
                }
            }
            else if (summaryText != null)
            {
                summaryText.text = "Result: steps not found";
            }
        }

        private void OnRestartClicked()
        {
            if (resultPanel != null)
                resultPanel.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartTraining();
                return;
            }

            if (scenarioController != null)
                scenarioController.StartScenario();
        }

        private void OnLobbyClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ReturnToLobby();
        }
    }
}