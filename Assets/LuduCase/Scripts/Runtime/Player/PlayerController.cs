using UnityEngine;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Handles First Person movement and camera rotation.
    /// Requirements: GameObject must have a CharacterController component.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        [Header("Movement Settings")]
        [Tooltip("Walking speed in units per second.")]
        [SerializeField] private float m_MoveSpeed = 5.0f;

        [Tooltip("Mouse look sensitivity.")]
        [SerializeField] private float m_MouseSensitivity = 2.0f;

        [Tooltip("Maximum vertical look angle (in degrees).")]
        [SerializeField] private float m_LookXLimit = 85.0f;

        [Header("References")]
        [SerializeField] private Camera m_PlayerCamera;

        // Private runtime fields
        private CharacterController m_CharacterController;
        private Vector3 m_MoveDirection = Vector3.zero;
        private float m_RotationX = 0;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();

            // Lock cursor for FPS experience
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (m_PlayerCamera == null)
            {
                m_PlayerCamera = Camera.main;
                if (m_PlayerCamera == null)
                {
                    Debug.LogError($"{name}: PlayerController requires a Camera!");
                }
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes WASD input and moves the character.
        /// </summary>
        private void HandleMovement()
        {
            // Calculate movement direction based on local transform
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Get Input
            float curSpeedX = m_MoveSpeed * Input.GetAxis("Vertical");
            float curSpeedY = m_MoveSpeed * Input.GetAxis("Horizontal");

            // Apply movement
            m_MoveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // Apply Gravity (Optional but good for feel)
            // Simple gravity hack since we don't have a jump yet
            if (!m_CharacterController.isGrounded)
            {
                m_MoveDirection.y += Physics.gravity.y * Time.deltaTime;
            }

            m_CharacterController.Move(m_MoveDirection * Time.deltaTime);
        }

        /// <summary>
        /// Processes Mouse X/Y input to rotate the camera and body.
        /// </summary>
        private void HandleMouseLook()
        {
            // Vertical Rotation (Camera only)
            m_RotationX += -Input.GetAxis("Mouse Y") * m_MouseSensitivity;
            m_RotationX = Mathf.Clamp(m_RotationX, -m_LookXLimit, m_LookXLimit);

            if (m_PlayerCamera != null)
            {
                m_PlayerCamera.transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
            }

            // Horizontal Rotation (Entire Player Body)
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * m_MouseSensitivity, 0);
        }

        #endregion
    }
}