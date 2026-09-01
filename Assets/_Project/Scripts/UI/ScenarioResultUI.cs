using System.Collections.Generic;
using _Project.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;
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

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            if (reports != null)
            {
                foreach (var report in reports)
                {
                    var item = Instantiate(itemPrefab, container);
                    var description = report.Step != null ? report.Step.Description : "Unknown step";
                    item.Setup(description, report.Status);
                }
            }
        }

        private void OnRestartClicked()
        {
            if (resultPanel != null)
                resultPanel.SetActive(false);

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