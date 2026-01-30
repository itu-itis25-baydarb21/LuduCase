using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        float HoldDuration { get; }

        bool Interact(GameObject interactor);

        // YENÝ EKLENEN METOTLAR:
        // Oyuncu nesneye bakmaya baþladýðýnda çalýþýr
        void OnFocus();

        // Oyuncu nesneden gözünü çektiðinde çalýþýr
        void OnLoseFocus();
    }
}