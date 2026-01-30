using InteractionSystem.Runtime.Core;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    // CHANGED: 'private' -> 'protected'
    [SerializeField] protected string m_InteractionPrompt = "Interact";

    [SerializeField] private float m_HoldDuration = 0f;

    public string InteractionPrompt => m_InteractionPrompt;
    public float HoldDuration => m_HoldDuration;

    public virtual bool Interact(GameObject interactor)
    {
        Debug.Log($"Interacting with: {gameObject.name}");
        return true;
    }
}