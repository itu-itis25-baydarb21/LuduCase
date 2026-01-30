using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class Chest : InteractableBase
    {
        #region Fields

        [Header("Chest State")]
        [SerializeField] private bool m_IsOpen = false;

        [Header("Visuals")]
        [SerializeField] private Transform m_LidVisual; // Drag the "Lid" child here
        [SerializeField] private Vector3 m_OpenRotation = new Vector3(-45, 0, 0);

        #endregion

        #region Methods

        public override bool Interact(GameObject interactor)
        {
            // If already open, do nothing
            if (m_IsOpen) return false;

            // Call base to log interaction
            base.Interact(interactor);

            m_IsOpen = true;
            Debug.Log("<color=gold>Chest Opened!</color> Found: Gold Coin");

            // Simple visual animation (Snap rotation)
            if (m_LidVisual != null)
            {
                m_LidVisual.localRotation = Quaternion.Euler(m_OpenRotation);
            }

            // Optional: Disable collider or interaction so it can't be used again
            // GetComponent<Collider>().enabled = false;

            return true;
        }

        #endregion
    }
}