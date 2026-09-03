using UnityEngine.EventSystems;

namespace _Project.Scripts.Interactions
{
    public class ClickTarget : BaseInteractable, IPointerClickHandler
    {
        // Mouse click event handler
        private void OnMouseDown()
        {
            TriggerInteraction();
        }
        
        // Pointer click event handler for UI elements
        public void OnPointerClick(PointerEventData eventData)
        {
            TriggerInteraction();
        }
        
        public void Click()
        {
            TriggerInteraction();
        }
    }
}