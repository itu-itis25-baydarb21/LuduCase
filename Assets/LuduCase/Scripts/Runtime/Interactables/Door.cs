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
        [SerializeField] private string m_RequiredKeyID = "RedKey";

        [Header("Visuals")]
        [SerializeField] private Transform m_DoorVisual;
        [SerializeField] private Vector3 m_OpenRotation = new Vector3(0, 90, 0);
        [SerializeField] private Vector3 m_ClosedRotation = Vector3.zero;

        [Header("Animation")]
        [Tooltip("Time in seconds to open/close the door.")]
        [SerializeField] private float m_AnimationDuration = 1.0f; // 1 second default

        // Private fields for animation state
        private Coroutine m_CurrentRoutine;
        private InteractionUI m_UI;

        #endregion

        #region Unity Methods

        private void Start()
        {
            m_UI = FindObjectOfType<InteractionUI>();
            UpdatePrompt();
        }

        #endregion

        #region Methods

        public override bool Interact(GameObject interactor)
        {
            if (m_IsLocked)
            {
                Inventory inventory = interactor.GetComponent<Inventory>();

                if (inventory != null && inventory.HasKey(m_RequiredKeyID))
                {
                    m_IsLocked = false;
                    Debug.Log($"<color=green>Door Unlocked</color>");
                    if (m_UI != null) m_UI.ShowFeedbackMessage("Door Unlocked", 2.0f);
                    UpdatePrompt();
                }
                else
                {
                    Debug.Log($"<color=red>Access Denied!</color>");
                    if (m_UI != null)
                    {
                        m_UI.ShowFeedbackMessage($"It's locked. You need {m_RequiredKeyID}", 3.0f);
                    }
                    return false;
                }
            }

            return base.Interact(interactor);
        }

        protected override void OnStateChanged(bool isOpen)
        {
            // Instead of setting rotation instantly, we start a smooth routine
            if (m_CurrentRoutine != null)
            {
                StopCoroutine(m_CurrentRoutine); // Stop any existing movement so they don't fight
            }

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
                // Calculate how far we are (0.0 to 1.0)
                float t = elapsed / m_AnimationDuration;

                // Optional: SmoothStep makes it start slow and end slow (like real physics)
                t = Mathf.SmoothStep(0f, 1f, t);

                // Slerp = Spherical Linear Interpolation (Best for rotations)
                m_DoorVisual.localRotation = Quaternion.Slerp(startRotation, target, t);

                yield return null; // Wait for the next frame
            }

            // Ensure we hit the exact target at the end
            m_DoorVisual.localRotation = target;
        }

        private void UpdatePrompt()
        {
            m_InteractionPrompt = m_IsActive ? "Press E to Close" : "Press E to Open";
        }

        #endregion
    }
}