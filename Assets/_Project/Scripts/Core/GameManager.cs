using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core
{
    /// <summary>
    /// High-level states of the application scene flow.
    /// </summary>
    public enum GameState
    {
        Lobby,
        Loading,
        Training
    }

    /// <summary>
    /// Application entry point that survives scene loads and owns the transition
    /// between the Lobby and Training scenes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Scene Names")]
        [SerializeField] private string lobbySceneName = "LobbyScene";
        [SerializeField] private string trainingSceneName = "TrainingScene";

        [Header("Loading")]
        [Tooltip("Keeps the loading screen visible for at least this long, " +
                 "so a fast local load doesn't just flash on screen.")]
        [SerializeField] private bool useMinimumLoadingTime = true;
        [SerializeField] private float minimumLoadingTime = 0.5f;

        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Lobby;

        /// <summary>
        /// Raised whenever CurrentState changes (including entering/leaving Loading)
        /// </summary>
        public event Action<GameState> OnStateChanged;

        /// <summary>
        /// Normalized (0..1) scene load progress, useful for a loading bar.
        /// </summary>
        public event Action<float> OnLoadingProgressChanged;

        public event Action OnLoadingStarted;
        public event Action OnLoadingCompleted;

        private bool _isLoading;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Loads the Training scene from the Lobby menu.
        /// </summary>
        public void StartTraining()
        {
            if (_isLoading) return;
            LoadScene(trainingSceneName, GameState.Training);
        }

        /// <summary>
        /// Reloads the Training scene.
        /// </summary>
        public void RestartTraining()
        {
            if (_isLoading || CurrentState != GameState.Training) return;
            LoadScene(trainingSceneName, GameState.Training);
        }

        /// <summary>
        /// Returns to the Lobby scene
        /// </summary>
        public void ReturnToLobby()
        {
            if (_isLoading) return;
            LoadScene(lobbySceneName, GameState.Lobby);
        }

        private void LoadScene(string sceneName, GameState targetState)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError($"[GameManager] Scene name for state {targetState} is not assigned.");
                return;
            }
            StartCoroutine(LoadSceneRoutine(sceneName, targetState));
        }

        private IEnumerator LoadSceneRoutine(string sceneName, GameState targetState)
        {
            _isLoading = true;
            CurrentState = GameState.Loading;
            OnLoadingStarted?.Invoke();
            OnStateChanged?.Invoke(CurrentState);

            var startTime = Time.time;
            var operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation != null)
            {
                operation.allowSceneActivation = false;
                
                while (operation.progress < 0.9f)
                {
                    OnLoadingProgressChanged?.Invoke(operation.progress / 0.9f);
                    yield return null;
                }

                if (useMinimumLoadingTime)
                {
                    var elapsed = Time.time - startTime;
                    if (elapsed < minimumLoadingTime)
                    {
                        yield return new WaitForSeconds(minimumLoadingTime - elapsed);
                    }
                }

                OnLoadingProgressChanged?.Invoke(1f);
                operation.allowSceneActivation = true;

                while (!operation.isDone)
                {
                    yield return null;
                }
            }

            _isLoading = false;
            CurrentState = targetState;
            OnStateChanged?.Invoke(CurrentState);
            OnLoadingCompleted?.Invoke();
        }
    }
}