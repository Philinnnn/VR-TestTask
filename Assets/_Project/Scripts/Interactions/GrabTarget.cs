using UnityEngine;

namespace _Project.Scripts.Interactions
{
    /// <summary>
    /// An object that can be picked up and carried around by the player.
    /// When grabbed, it will follow a specified hold point
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class GrabTarget : BaseInteractable
    {
        [Tooltip("How quickly the object catches up to the hold point while carried.")]
        [SerializeField] private float followSpeed = 20f;

        private Rigidbody _rigidbody;
        private Transform _holdPoint;
        private bool _wasKinematic;
        private bool _hadGravity;

        public bool IsGrabbed { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!IsGrabbed || !_holdPoint) return;
            var targetPosition = Vector3.Lerp(_rigidbody.position, _holdPoint.position, followSpeed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(targetPosition);
        }
        
        public void Grab(Transform holdPoint)
        {
            if (IsGrabbed || !holdPoint) return;

            _holdPoint = holdPoint;
            IsGrabbed = true;

            _wasKinematic = _rigidbody.isKinematic;
            _hadGravity = _rigidbody.useGravity;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;

            TriggerInteraction();
        }
        
        public void Release()
        {
            if (!IsGrabbed) return;

            IsGrabbed = false;
            _holdPoint = null;

            _rigidbody.isKinematic = _wasKinematic;
            _rigidbody.useGravity = _hadGravity;
        }
    }
}