using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Move / Look Settings")]
        public float MoveSpeed = 4.0f;
        public float RotationSpeed = 1.0f;
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        [Header("Gravity Settings")]
        public float Gravity = -9.81f;                // Gravity acceleration
        public float GroundedStickForce = -2.0f;      // Keeps character grounded
        public float TerminalVelocity = -53.0f;       // Max fall speed

        [Header("Jump Settings")]
        public float JumpHeight = 1.2f;               // Desired jump height
        public float CoyoteTime = 0.1f;               // Grace period after leaving ground
        public float JumpBufferTime = 0.1f;           // Input buffer time for jump

        private float _verticalVelocity;
        private float _cinemachineTargetPitch;
        private float _rotationVelocity;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private PlayerInput _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;
        private float _lastGroundedTime = -999f;
        private float _lastJumpPressedTime = -999f;

        private bool IsCurrentDeviceMouse => true;

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInput>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
        }

        private void Update()
        {
            CacheGroundedTime();
            ReadJumpInput();
            TryJump();

            ApplyGravity();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CacheGroundedTime()
        {
            if (_controller.isGrounded)
            {
                _lastGroundedTime = Time.time;
            }
        }

        private void ReadJumpInput()
        {
            bool jumpPressed = false;

#if ENABLE_INPUT_SYSTEM
            // New Input System
            if (Keyboard.current != null)
            {
                jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            }
            else
#endif
            {
                // Legacy Input System
                jumpPressed = Input.GetKeyDown(KeyCode.Space);
            }

            if (jumpPressed)
            {
                _lastJumpPressedTime = Time.time;
            }
        }

        private void TryJump()
        {
            bool canCoyote = (Time.time - _lastGroundedTime) <= CoyoteTime;
            bool inBuffer = (Time.time - _lastJumpPressedTime) <= JumpBufferTime;

            if (canCoyote && inBuffer)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                _lastJumpPressedTime = -999f;
                _lastGroundedTime = -999f;
            }
        }

        private void ApplyGravity()
        {
            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = GroundedStickForce;
            }
            else
            {
                _verticalVelocity += Gravity * Time.deltaTime;
                if (_verticalVelocity < TerminalVelocity)
                    _verticalVelocity = TerminalVelocity;
            }
        }

        private void Move()
        {
            Vector3 moveDirection = new Vector3(_input.move.x, 0f, _input.move.y);

            if (moveDirection.magnitude >= _threshold)
            {
                moveDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }
            else
            {
                moveDirection = Vector3.zero;
            }

            Vector3 velocity = moveDirection * MoveSpeed + Vector3.up * _verticalVelocity;
            _controller.Move(velocity * Time.deltaTime);
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

                _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, BottomClamp, TopClamp);

                if (CinemachineCameraTarget != null)
                {
                    CinemachineCameraTarget.transform.localRotation =
                        Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
                }

                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }
    }
}
