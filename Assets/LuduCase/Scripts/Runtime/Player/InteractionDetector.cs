using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.UI;

namespace InteractionSystem.Runtime.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3.0f; 
        [SerializeField] private float m_DetectionRange = 10.0f;  
        [SerializeField] private LayerMask m_InteractableLayer;

        [Header("Input Settings")]
        [Tooltip("The key used to interact with objects.")]
        [SerializeField] private KeyCode m_InteractionKey = KeyCode.E;

        [Header("References")]
        [SerializeField] private Camera m_PlayerCamera;
        [SerializeField] private InteractionUI m_UI;

        // Private State
        private IInteractable m_CurrentInteractable;
        private float m_CurrentHoldTime = 0f;

        private bool m_IsInRange = false;

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

            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_DetectionRange, m_InteractableLayer))
            {
                IInteractable interactable = hitInfo.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    if (m_CurrentInteractable != interactable)
                    {
                        if (m_CurrentInteractable != null) m_CurrentInteractable.OnLoseFocus();

                        m_CurrentInteractable = interactable;
                        m_CurrentInteractable.OnFocus();
                    }

                    float distance = hitInfo.distance;
                    m_IsInRange = distance <= m_InteractionRange;

                    string dynamicPrompt = interactable.InteractionPrompt.Replace("{KEY}", m_InteractionKey.ToString());

                    if (m_IsInRange)
                    {
                        if (m_UI != null) m_UI.ShowPrompt(dynamicPrompt, false);
                    }
                    else
                    {
                        if (m_UI != null) m_UI.ShowPrompt($"{dynamicPrompt} (Too Far)", true);
                    }

                    return;
                }
            }

            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnLoseFocus();
                m_CurrentInteractable = null;
                m_IsInRange = false; // Temizle
                if (m_UI != null) m_UI.HidePrompt();
            }
        }

        private void HandleInput()
        {
            if (m_CurrentInteractable != null && m_IsInRange)
            {
                // HOLD INTERACTION
                if (m_CurrentInteractable.HoldDuration > 0)
                {
                    if (Input.GetKey(m_InteractionKey))
                    {
                        m_CurrentHoldTime += Time.deltaTime;

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
                    if (Input.GetKeyDown(m_InteractionKey))
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