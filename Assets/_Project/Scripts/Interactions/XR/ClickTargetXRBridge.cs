using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _Project.Scripts.Interactions.XR
{
    /// <summary>
    /// Bridges an XRSimpleInteractable to an existing ClickTarget, so pulling
    /// the trigger while a ray interactor points at the object counts as a
    /// click - the same event ClickTarget already raises for OnMouseDown and
    /// UI pointer clicks.
    ///
    /// Requires XR Interaction Toolkit 3.x namespaces (see GrabTargetXRBridge
    /// for the 2.x note).
    /// </summary>
    [RequireComponent(typeof(ClickTarget))]
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class ClickTargetXRBridge : MonoBehaviour
    {
        private ClickTarget _clickTarget;
        private XRSimpleInteractable _interactable;

        private void Awake()
        {
            _clickTarget = GetComponent<ClickTarget>();
            _interactable = GetComponent<XRSimpleInteractable>();
        }

        private void OnEnable()
        {
            _interactable.selectEntered.AddListener(HandleSelectEntered);
        }

        private void OnDisable()
        {
            _interactable.selectEntered.RemoveListener(HandleSelectEntered);
        }

        private void HandleSelectEntered(SelectEnterEventArgs args)
        {
            _clickTarget.Click();
        }
    }
}
