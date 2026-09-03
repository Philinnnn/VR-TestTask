using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _Project.Scripts.Interactions.XR
{
    /// <summary>
    /// Bridges an XRSimpleInteractable to an existing GrabTarget, so a VR
    /// controller can pick it up the same way MouseGrabber does on desktop.
    /// GrabTarget keeps owning all the actual grab/follow physics - this
    /// component only forwards XRI's select events to Grab()/Release().
    ///
    /// Requires XR Interaction Toolkit 3.x namespaces. On 2.x, drop the
    /// "Interactables" using and reference XRSimpleInteractable/
    /// SelectEnterEventArgs/SelectExitEventArgs directly from
    /// UnityEngine.XR.Interaction.Toolkit.
    /// </summary>
    [RequireComponent(typeof(GrabTarget))]
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class GrabTargetXRBridge : MonoBehaviour
    {
        private GrabTarget _grabTarget;
        private XRSimpleInteractable _interactable;

        private void Awake()
        {
            _grabTarget = GetComponent<GrabTarget>();
            _interactable = GetComponent<XRSimpleInteractable>();
        }

        private void OnEnable()
        {
            _interactable.selectEntered.AddListener(HandleSelectEntered);
            _interactable.selectExited.AddListener(HandleSelectExited);
        }

        private void OnDisable()
        {
            _interactable.selectEntered.RemoveListener(HandleSelectEntered);
            _interactable.selectExited.RemoveListener(HandleSelectExited);
        }

        private void HandleSelectEntered(SelectEnterEventArgs args)
        {
            // Follow the controller's own attach point (accounts for grab-pose
            // offsets configured on the interactor), not just its raw transform.
            var holdPoint = args.interactorObject.GetAttachTransform(_interactable);
            _grabTarget.Grab(holdPoint);
        }

        private void HandleSelectExited(SelectExitEventArgs args)
        {
            _grabTarget.Release();
        }
    }
}
