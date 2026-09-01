using TMPro;
using UnityEngine;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.UI
{
    public class ScenarioResultItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text stepDescriptionText;
        [SerializeField] private TMP_Text statusText;

        public void Setup(string description, StepStatus status)
        {
            if (stepDescriptionText != null) 
                stepDescriptionText.text = description;

            if (statusText != null)
            {
                statusText.text = status switch
                {
                    StepStatus.Success => "<color=green>Success</color>",
                    StepStatus.Failed => "<color=red>Failed</color>",
                    StepStatus.Skipped => "<color=yellow>Skipped</color>",
                    _ => status.ToString()
                };
            }
        }
    }
}