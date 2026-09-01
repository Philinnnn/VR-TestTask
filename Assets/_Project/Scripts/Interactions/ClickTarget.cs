namespace _Project.Scripts.Interactions
{
    public class ClickTarget : BaseInteractable
    {
        // Mouse click event handler
        private void OnMouseDown()
        {
            TriggerInteraction();
        }
        
        // Pointer click event handler for UI elements
        public void OnPointerClick()
        {
            TriggerInteraction();
        }
    }
}