using _Project.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Lobby scene menu: a single "Start Training" button that hands off
    /// scene loading to GameManager
    /// </summary>
    public class LobbyMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button startTrainingButton;
        [SerializeField] private TMP_Text statusText;

        [Header("Status Labels")]
        [SerializeField] private string idleLabel = "Начать тренировку";
        [SerializeField] private string loadingLabel = "Загрузка...";

        private void Awake()
        {
            SetStatus(idleLabel);
        }

        private void OnEnable()
        {
            if (startTrainingButton != null)
                startTrainingButton.onClick.AddListener(OnStartTrainingClicked);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLoadingStarted += HandleLoadingStarted;
                GameManager.Instance.OnLoadingCompleted += HandleLoadingCompleted;
            }
        }

        private void OnDisable()
        {
            if (startTrainingButton != null)
                startTrainingButton.onClick.RemoveListener(OnStartTrainingClicked);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLoadingStarted -= HandleLoadingStarted;
                GameManager.Instance.OnLoadingCompleted -= HandleLoadingCompleted;
            }
        }

        private void OnStartTrainingClicked()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("[LobbyMenuUI] GameManager.Instance is null. " +
                               "Make sure a GameManager exists in the scene (or was carried over via DontDestroyOnLoad).");
                return;
            }

            GameManager.Instance.StartTraining();
        }

        private void HandleLoadingStarted()
        {
            if (startTrainingButton)
                startTrainingButton.interactable = false;

            SetStatus(loadingLabel);
        }

        private void HandleLoadingCompleted()
        {
            // The Lobby scene (and this object) is being unloaded right after this
            // fires when leaving the Lobby, so this mainly matters if loading was
            // canceled or this panel is reused - keep state consistent regardless.
            if (startTrainingButton)
                startTrainingButton.interactable = true;

            SetStatus(idleLabel);
        }

        private void SetStatus(string label)
        {
            if (statusText)
                statusText.text = label;
        }
    }
}