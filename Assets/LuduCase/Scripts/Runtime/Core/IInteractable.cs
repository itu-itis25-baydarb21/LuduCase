using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Interface defining the contract for all objects that the player can interact with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Gets the prompt message to be displayed on the UI (e.g., "Press E to Open").
        /// </summary>
        string InteractionPrompt { get; }

        /// <summary>
        /// Executes the interaction logic.
        /// </summary>
        /// <param name="interactor">The GameObject performing the interaction (usually the player).</param>
        /// <returns>True if interaction was successful.</returns>
        bool Interact(GameObject interactor);
    }
}