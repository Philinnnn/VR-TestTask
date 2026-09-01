using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Interactions
{
    [RequireComponent(typeof(Button))]
    public class ButtonUITarget : BaseInteractable
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TriggerInteraction);
        }

        private void OnDestroy()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(TriggerInteraction);
            }
        }
    }
}