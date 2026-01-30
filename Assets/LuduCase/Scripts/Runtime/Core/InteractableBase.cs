using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private string m_InteractionPrompt = "Interact";

        // NEW: Added this field so you can set it to "2.0" for the Chest
        [SerializeField] private float m_HoldDuration = 0f;

        public string InteractionPrompt => m_InteractionPrompt;

        // NEW: Implementing the property required by the interface
        public float HoldDuration => m_HoldDuration;

        public virtual bool Interact(GameObject interactor)
        {
            Debug.Log($"Interacting with: {gameObject.name}");
            return true;
        }
    }
}