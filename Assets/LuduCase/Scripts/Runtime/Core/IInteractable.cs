using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    public interface IInteractable
    {
        // OLD: just the prompt
        string InteractionPrompt { get; }

        // NEW: We added this line for the chest mechanic!
        float HoldDuration { get; }

        bool Interact(GameObject interactor);
    }
}