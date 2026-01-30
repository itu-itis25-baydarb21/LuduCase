using UnityEngine;
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class Door : ToggleInteractable
    {
        #region Fields

        [Header("Lock Settings")]
        [SerializeField] private bool m_IsLocked = false;

        [Tooltip("The Key ID required to unlock this door (must match KeyItem).")]
        [SerializeField] private string m_RequiredKeyID = "GeneralKey";

        [Header("Animation (Simple)")]
        [SerializeField] private Transform m_DoorVisual;
        [SerializeField] private Vector3 m_OpenRotation = new Vector3(0, 90, 0);
        [SerializeField] private Vector3 m_ClosedRotation = Vector3.zero;

        #endregion

        #region Methods

        public override bool Interact(GameObject interactor)
        {
            // 1. If locked, try to unlock
            if (m_IsLocked)
            {
                Inventory inventory = interactor.GetComponent<Inventory>();

                if (inventory != null && inventory.HasKey(m_RequiredKeyID))
                {
                    m_IsLocked = false;
                    Debug.Log($"<color=green>Door Unlocked</color> with {m_RequiredKeyID}");
                    // Update prompt to normal state
                    // (Real-world: We would update the prompt text here dynamically)
                }
                else
                {
                    Debug.Log($"<color=red>Locked!</color> Requires key: {m_RequiredKeyID}");
                    return false; // Interaction failed
                }
            }

            // 2. If unlocked (or just unlocked), toggle the door
            return base.Interact(interactor);
        }

        protected override void OnStateChanged(bool isOpen)
        {
            // Simple visual rotation
            if (m_DoorVisual != null)
            {
                m_DoorVisual.localRotation = Quaternion.Euler(isOpen ? m_OpenRotation : m_ClosedRotation);
            }

            Debug.Log($"Door is now {(isOpen ? "OPEN" : "CLOSED")}");
        }

        #endregion
    }
}