using TMPro;
using UnityEngine;
using _Project.Scripts.Scenario.Data;

namespace _Project.Scripts.UI
{
    public class ScenarioResultItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text stepDescriptionText;
        [SerializeField] private TMP_Text statusText;

        public void Setup(int index, string description, StepStatus status)
        {
            if (stepDescriptionText != null) 
                stepDescriptionText.text = $"{index + 1}. {description}";

            if (statusText != null)
            {
                statusText.text = status switch
                {
                    StepStatus.Success => "<color=#4CAF50>Success</color>",
                    StepStatus.Failed => "<color=#F44336>Failed</color>",
                    StepStatus.Skipped => "<color=#FFC107>Skipped</color>",
                    _ => status.ToString()
                };
            }

            var itemRect = transform as RectTransform;
            if (itemRect != null)
            {
                itemRect.anchorMin = new Vector2(0f, 1f);
                itemRect.anchorMax = new Vector2(1f, 1f);
                itemRect.pivot = new Vector2(0.5f, 1f);
                itemRect.anchoredPosition = Vector2.zero;
                itemRect.sizeDelta = new Vector2(0f, 44f);
            }

            if (stepDescriptionText != null)
            {
                var stepRect = stepDescriptionText.rectTransform;
                stepRect.anchorMin = new Vector2(0f, 0f);
                stepRect.anchorMax = new Vector2(0.75f, 1f);
                stepRect.pivot = new Vector2(0f, 0.5f);
                stepRect.offsetMin = new Vector2(8f, 0f);
                stepRect.offsetMax = new Vector2(-8f, 0f);
                stepDescriptionText.alignment = TextAlignmentOptions.Left;
                stepDescriptionText.enableWordWrapping = true;
            }

            if (statusText != null)
            {
                var statusRect = statusText.rectTransform;
                statusRect.anchorMin = new Vector2(0.75f, 0f);
                statusRect.anchorMax = new Vector2(1f, 1f);
                statusRect.pivot = new Vector2(1f, 0.5f);
                statusRect.offsetMin = new Vector2(8f, 0f);
                statusRect.offsetMax = new Vector2(-8f, 0f);
                statusText.alignment = TextAlignmentOptions.Right;
                statusText.enableWordWrapping = false;
            }
        }
    }
}