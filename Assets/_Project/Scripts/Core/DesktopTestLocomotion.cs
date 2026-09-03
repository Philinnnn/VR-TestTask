using UnityEngine;

namespace _Project.Scripts.Core
{
    /// <summary>
    /// Desktop-only stand-in for VR locomotion, so the scenario can be walked
    /// through with mouse + keyboard while no headset is connected. WASD
    /// moves, mouse looks around.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class DesktopTestLocomotion : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Look")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float maxLookAngle = 80f;

        private CharacterController _controller;
        private float _verticalVelocity;
        private float _cameraPitch;
        private bool _cursorLocked;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            SetCursorLocked(true);
        }

        private void OnDisable()
        {
            SetCursorLocked(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
                SetCursorLocked(!_cursorLocked);

            if (_cursorLocked)
                HandleLook();

            HandleMove();
        }

        private void SetCursorLocked(bool locked)
        {
            _cursorLocked = locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }

        private void HandleLook()
        {
            if (!cameraTransform) return;

            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            transform.Rotate(Vector3.up * mouseX);

            _cameraPitch = Mathf.Clamp(_cameraPitch - mouseY, -maxLookAngle, maxLookAngle);
            cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
        }

        private void HandleMove()
        {
            var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            var move = transform.TransformDirection(input.normalized) * moveSpeed;

            if (_controller.isGrounded)
                _verticalVelocity = -0.5f;
            else
                _verticalVelocity += gravity * Time.deltaTime;

            move.y = _verticalVelocity;
            _controller.Move(move * Time.deltaTime);
        }
    }
}
