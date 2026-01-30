using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3.0f;
        [SerializeField] private float m_DetectionRange = 10.0f;  // Can see (NEW)
        [SerializeField] private LayerMask m_InteractableLayer;

        [Header("References")]
        [SerializeField] private Camera m_PlayerCamera;
        [SerializeField] private InteractionSystem.Runtime.UI.InteractionUI m_UI;

        // Private State
        private IInteractable m_CurrentInteractable;
        private float m_CurrentHoldTime = 0f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_PlayerCamera == null) m_PlayerCamera = Camera.main;
        }

        private void Update()
        {
            PerformDetection();
            HandleInput();
        }

        #endregion

        #region Methods

        private void PerformDetection()
        {
            Ray ray = new Ray(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward);

            // 1. Raycast uses the LONGER detection range
            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_DetectionRange, m_InteractableLayer))
            {
                IInteractable interactable = hitInfo.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    m_CurrentInteractable = interactable;

                    // 2. Check Distance
                    float distance = hitInfo.distance;
                    bool isOutOfRange = distance > m_InteractionRange;

                    if (isOutOfRange)
                    {
                        // Case A: Too Far -> Show Warning
                        string message = $"{interactable.InteractionPrompt} (Too Far)";
                        if (m_UI != null) m_UI.ShowPrompt(message, true); // true = Warning Color

                        // Prevent interaction by clearing current target locally for Input, 
                        // OR handle it in HandleInput. 
                        // Better approach: Keep m_CurrentInteractable but flag it as unusable.
                        m_CurrentInteractable = null; // Disables input
                    }
                    else
                    {
                        // Case B: In Range -> Normal Prompt
                        if (m_UI != null) m_UI.ShowPrompt(interactable.InteractionPrompt, false);
                    }

                    return;
                }
            }

            // If nothing hit
            if (m_CurrentInteractable != null || (m_UI != null)) // Reset everything
            {
                m_CurrentInteractable = null;
                if (m_UI != null)
                {
                    m_UI.HidePrompt();
                    m_UI.UpdateProgress(0);
                }
            }
        }

        private void HandleInput()
        {
            if (m_CurrentInteractable != null)
            {
                // HOLD INTERACTION
                if (m_CurrentInteractable.HoldDuration > 0)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        m_CurrentHoldTime += Time.deltaTime;

                        // Update Progress Bar
                        float progress = m_CurrentHoldTime / m_CurrentInteractable.HoldDuration;
                        if (m_UI != null) m_UI.UpdateProgress(progress);

                        if (m_CurrentHoldTime >= m_CurrentInteractable.HoldDuration)
                        {
                            m_CurrentInteractable.Interact(gameObject);
                            m_CurrentHoldTime = 0;
                            if (m_UI != null) m_UI.UpdateProgress(0);
                        }
                    }
                    else
                    {
                        m_CurrentHoldTime = 0;
                        if (m_UI != null) m_UI.UpdateProgress(0);
                    }
                }
                // INSTANT INTERACTION
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        m_CurrentInteractable.Interact(gameObject);
                    }
                }
            }
            else
            {
                m_CurrentHoldTime = 0;
                if (m_UI != null) m_UI.UpdateProgress(0);
            }
        }

        #endregion
    }
}