using UnityEngine;
using _Project.Scripts.Scenario;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class ScenarioAudioFeedback : MonoBehaviour
    {
        [SerializeField] private ScenarioController scenarioController;
        [SerializeField] private AudioClip successClip;
        [SerializeField] private AudioClip violationClip;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f; // 2D UI-style cue, not positioned at the interacted object
        }

        private void OnEnable()
        {
            if (scenarioController == null) return;

            scenarioController.OnStepCompleted += HandleStepCompleted;
            scenarioController.OnSequenceError += HandleSequenceError;
        }

        private void OnDisable()
        {
            if (scenarioController == null) return;

            scenarioController.OnStepCompleted -= HandleStepCompleted;
            scenarioController.OnSequenceError -= HandleSequenceError;
        }

        private void HandleStepCompleted(StepSO step, StepStatus status)
        {
            switch (status)
            {
                case StepStatus.Success:
                    PlayClip(successClip);
                    break;
                case StepStatus.Failed:
                    PlayClip(violationClip);
                    break;
            }
        }

        private void HandleSequenceError()
        {
            PlayClip(violationClip);
        }

        private void PlayClip(AudioClip clip)
        {
            if (clip == null || _audioSource == null) return;
            _audioSource.PlayOneShot(clip);
        }
    }
}