using System.Collections;
using UnityEngine;
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.UI;

namespace InteractionSystem.Runtime.Interactables
{
    public class Door : ToggleInteractable
    {
        #region Fields

        [Header("Lock Settings")]
        [SerializeField] private bool m_IsLocked = false;

        [Tooltip("The specific Key Data asset required to unlock this door.")]
        [SerializeField] private ItemData m_RequiredKey;

        [Header("Visuals")]
        [Tooltip("The Hinge object that rotates.")]
        [SerializeField] private Transform m_DoorVisual;

        [Tooltip("The MeshRenderer of the door model (used to apply color tint).")]
        [SerializeField] private Renderer m_DoorRenderer;

        [SerializeField] private Vector3 m_OpenRotation = new Vector3(0, 90, 0);
        [SerializeField] private Vector3 m_ClosedRotation = Vector3.zero;

        [Header("Animation")]
        [Tooltip("Time in seconds to open/close the door.")]
        [SerializeField] private float m_AnimationDuration = 1.0f;

        // Private state
        private Coroutine m_CurrentRoutine;
        private InteractionUI m_UI;

        #endregion

        #region Unity Methods

        private void Start()
        {
            // Find the global UI manager
            m_UI = FindObjectOfType<InteractionUI>();

            // If the door is locked and requires a specific key, tint the door color to match the key
            if (m_IsLocked && m_RequiredKey != null && m_DoorRenderer != null)
            {
                m_DoorRenderer.material.color = m_RequiredKey.TintColor;
            }

            UpdatePrompt();
        }

        #endregion

        #region Methods

        public override bool Interact(GameObject interactor)
        {
            // 1. Logic for Locked Door
            if (m_IsLocked)
            {
                // Safely get ID from the data asset
                string requiredID = (m_RequiredKey != null) ? m_RequiredKey.ID : "";

                Inventory inventory = interactor.GetComponent<Inventory>();

                // Check if player has the correct Key ID
                if (inventory != null && inventory.HasKey(requiredID))
                {
                    // Success: Unlock the door
                    m_IsLocked = false;
                    Debug.Log($"<color=green>Door Unlocked</color>");

                    // Show success feedback
                    if (m_UI != null) m_UI.ShowFeedbackMessage("Door Unlocked", 2.0f);

                    UpdatePrompt();
                }
                else
                {
                    // Failure: Get display name for better feedback
                    string keyName = (m_RequiredKey != null) ? m_RequiredKey.DisplayName : "Key";

                    Debug.Log($"<color=red>Access Denied!</color>");

                    // Show failure feedback on the center of the screen
                    if (m_UI != null)
                    {
                        m_UI.ShowFeedbackMessage($"Locked! You need the {keyName}", 3.0f);
                    }
                    return false; // Stop interaction here
                }
            }

            // 2. Logic for Unlocked Door (Toggle Open/Close)
            return base.Interact(interactor);
        }

        protected override void OnStateChanged(bool isOpen)
        {
            // Stop any existing animation to prevent conflict
            if (m_CurrentRoutine != null) StopCoroutine(m_CurrentRoutine);

            Quaternion targetRotation = Quaternion.Euler(isOpen ? m_OpenRotation : m_ClosedRotation);
            m_CurrentRoutine = StartCoroutine(AnimateRotation(targetRotation));

            UpdatePrompt();
        }

        private IEnumerator AnimateRotation(Quaternion target)
        {
            Quaternion startRotation = m_DoorVisual.localRotation;
            float elapsed = 0f;

            while (elapsed < m_AnimationDuration)
            {
                elapsed += Time.deltaTime;

                // Calculate progress (0.0 to 1.0)
                float t = elapsed / m_AnimationDuration;

                // Apply smoothing (Ease-In / Ease-Out)
                t = Mathf.SmoothStep(0f, 1f, t);

                // Interpolate rotation
                m_DoorVisual.localRotation = Quaternion.Slerp(startRotation, target, t);

                yield return null; // Wait for next frame
            }

            // Ensure exact final rotation
            m_DoorVisual.localRotation = target;
        }

        private void UpdatePrompt()
        {
            // Change prompt text based on state
            // If locked, the prompt will just say "Open", but interaction will fail and show the error message.
            m_InteractionPrompt = m_IsActive ? "Press E to Close" : "Press E to Open";
        }

        #endregion
    }
}