using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Abstract base class for all interactable objects.
    /// Handles common data like the interaction prompt.
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Fields

        [Tooltip("The message displayed to the player (e.g., 'Press E to Open').")]
        [SerializeField] private string m_InteractionPrompt = "Interact";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the interaction prompt message.
        /// </summary>
        public string InteractionPrompt => m_InteractionPrompt;

        #endregion

        #region Methods

        /// <summary>
        /// Base interaction logic. Should be overridden by derived classes.
        /// </summary>
        /// <param name="interactor">The GameObject performing the interaction.</param>
        /// <returns>True if interaction was handled.</returns>
        public virtual bool Interact(GameObject interactor)
        {
            // Debug log to confirm the base method was called (can be removed in release)
            Debug.Log($"Interacting with: {gameObject.name}");
            return true;
        }

        #endregion
    }
}