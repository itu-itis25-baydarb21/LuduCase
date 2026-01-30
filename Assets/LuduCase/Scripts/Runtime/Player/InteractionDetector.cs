using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.UI;

namespace InteractionSystem.Runtime.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3.0f; // Etkileþim (Tuþa basma) mesafesi
        [SerializeField] private float m_DetectionRange = 10.0f;  // Görme (Outline) mesafesi
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

        // YENÝ: Oyuncu objeye yeterince yakýn mý?
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

            // Filtreli Raycast
            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_DetectionRange, m_InteractableLayer))
            {
                IInteractable interactable = hitInfo.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    // 1. ODAKLANMA (Outline)
                    if (m_CurrentInteractable != interactable)
                    {
                        if (m_CurrentInteractable != null) m_CurrentInteractable.OnLoseFocus();

                        m_CurrentInteractable = interactable;
                        m_CurrentInteractable.OnFocus();
                    }

                    // 2. MESAFE KONTROLÜ (WARNING ÇÖZÜMÜ BURADA)
                    float distance = hitInfo.distance;
                    m_IsInRange = distance <= m_InteractionRange; // Menzil içinde miyiz?

                    // 3. UI GÜNCELLEME
                    string dynamicPrompt = interactable.InteractionPrompt.Replace("{KEY}", m_InteractionKey.ToString());

                    if (m_IsInRange)
                    {
                        // Menzildeyiz: Normal Beyaz Yazý
                        if (m_UI != null) m_UI.ShowPrompt(dynamicPrompt, false);
                    }
                    else
                    {
                        // Uzaktayýz: Kýrmýzý "Too Far" uyarýsý
                        if (m_UI != null) m_UI.ShowPrompt($"{dynamicPrompt} (Too Far)", true);
                    }

                    return;
                }
            }

            // Hiçbir þeye bakmýyorsak temizle
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
            // Input alabilmek için hem obje olmalý HEM DE menzilde (InRange) olmalýyýz
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
                // Menzilden çýkýnca hold süresini sýfýrla
                m_CurrentHoldTime = 0;
                if (m_UI != null) m_UI.UpdateProgress(0);
            }
        }

        #endregion
    }
}