using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Base class for objects that toggle between two states (e.g., Switches, Doors).
    /// </summary>
    public abstract class ToggleInteractable : InteractableBase
    {
        #region Fields

        [Tooltip("Current state of the object. True = On/Open, False = Off/Closed.")]
        [SerializeField] protected bool m_IsActive;

        #endregion

        #region Methods

        public override bool Interact(GameObject interactor)
        {
            base.Interact(interactor);

            // Toggle the state
            m_IsActive = !m_IsActive;

            OnStateChanged(m_IsActive);

            return true;
        }

        /// <summary>
        /// Called when the state changes. Can be overridden for visuals/logic.
        /// </summary>
        protected virtual void OnStateChanged(bool newState)
        {
            // Debug logic, override in child classes
            Debug.Log($"Toggled state to: {newState}");
        }

        #endregion
    }
}