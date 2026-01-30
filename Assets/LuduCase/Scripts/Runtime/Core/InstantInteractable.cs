using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Represents an interactable that triggers immediately upon input (e.g., pickups, buttons).
    /// </summary>
    public class InstantInteractable : InteractableBase
    {
        #region Methods

        /// <summary>
        /// Triggers the instant interaction.
        /// </summary>
        /// <param name="interactor">The player object.</param>
        /// <returns>True if successful.</returns>
        public override bool Interact(GameObject interactor)
        {
            base.Interact(interactor);

            // Specific logic will be added by inheriting classes like KeyItem,
            // or we can add UnityEvents here if we want a generic "Button".
            Debug.Log($"<color=cyan>Instant Interaction:</color> {name}");

            return true;
        }

        #endregion
    }
}