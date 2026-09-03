using UnityEngine;

namespace _Project.Scripts.Interactions
{
    /// <summary>
    /// Editor/desktop-only stand-in for a VR hand: raycasts from the mouse
    /// position, and holding the mouse button while looking at a GrabTarget
    /// picks it up and carries it in front of the camera until released.
    /// </summary>
    public class MouseGrabber : MonoBehaviour
    {
        [SerializeField] private Camera sourceCamera;
        [SerializeField] private float grabDistance = 2f;
        [SerializeField] private float maxRayDistance = 5f;
        [SerializeField] private LayerMask grabbableLayers = ~0;

        private Transform _holdPoint;
        private GrabTarget _heldTarget;

        private void Awake()
        {
            if (sourceCamera == null)
                sourceCamera = Camera.main;
            
            var holdPointGo = new GameObject("MouseGrabber_HoldPoint");
            holdPointGo.transform.SetParent(transform);
            _holdPoint = holdPointGo.transform;
        }

        private void Update()
        {
            if (!sourceCamera) return;

            _holdPoint.position = sourceCamera.transform.position + sourceCamera.transform.forward * grabDistance;

            if (Input.GetMouseButtonDown(0))
            {
                TryGrab();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ReleaseCurrent();
            }
        }

        private void TryGrab()
        {
            var ray = new Ray(sourceCamera.transform.position, sourceCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, maxRayDistance, grabbableLayers))
            {
                var target = hit.collider.GetComponentInParent<GrabTarget>();
                if (target)
                {
                    _heldTarget = target;
                    _heldTarget.Grab(_holdPoint);
                }
            }
        }

        private void ReleaseCurrent()
        {
            if (_heldTarget)
            {
                _heldTarget.Release();
                _heldTarget = null;
            }
        }
    }
}