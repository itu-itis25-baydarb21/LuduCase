using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Handles raycasting to detect IInteractable objects in front of the player.
    /// </summary>
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection Settings")]
        [Tooltip("The range within which the player can interact with objects.")]
        [SerializeField] private float m_InteractionRange = 3.0f;

        [Tooltip("Layers that block the interaction raycast (e.g., Default, Interactable).")]
        [SerializeField] private LayerMask m_InteractableLayer;

        [Header("References")]
        [Tooltip("The camera used for raycasting. If null, uses Camera.main.")]
        [SerializeField] private Camera m_PlayerCamera;

        // Non-serialized private field
        private IInteractable m_CurrentInteractable;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Fallback if camera is not assigned
            if (m_PlayerCamera == null)
            {
                m_PlayerCamera = Camera.main;
                Debug.LogWarning($"{name}: Player Camera not assigned. Falling back to Camera.main.");
            }
        }

        private void Update()
        {
            PerformDetection();
            HandleInput();
        }

        private void OnDrawGizmos()
        {
            // Visual debugging for the interaction range
            if (m_PlayerCamera != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward * m_InteractionRange);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Casts a ray to detect interactable objects.
        /// </summary>
        private void PerformDetection()
        {
            Ray ray = new Ray(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward);

            // Check for hit within range
            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_InteractionRange, m_InteractableLayer))
            {
                // Try to get the IInteractable component from the hit object
                IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    if (m_CurrentInteractable != interactable)
                    {
                        m_CurrentInteractable = interactable;
                        // TODO: Connect this to UI in Phase 6
                        Debug.Log($"<color=green>Detected:</color> {m_CurrentInteractable.InteractionPrompt}");
                    }
                    return;
                }
            }

            // If we hit nothing or non-interactable, clear current
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable = null;
                Debug.Log("<color=red>Lost Interaction Target</color>");
            }
        }

        /// <summary>
        /// Checks for input to trigger the interaction.
        /// </summary>
        private void HandleInput()
        {
            // "Fire1" is usually Left Click, or you can map "Interact" to 'E' in Input Manager
            if (Input.GetKeyDown(KeyCode.E) && m_CurrentInteractable != null)
            {
                m_CurrentInteractable.Interact(gameObject);
            }
        }

        #endregion
    }
}