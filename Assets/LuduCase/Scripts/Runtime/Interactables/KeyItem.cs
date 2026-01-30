using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.Interactables
{
    public class KeyItem : InteractableBase
    {
        [Header("Item Data")]
        [SerializeField] private ItemData m_ItemData;

        [Header("Visuals")]
        // Anahtarýn renk deðiþtirecek parçasý (Mesh Renderer)
        [SerializeField] private Renderer m_Renderer;

        private void Start()
        {
            // Oyun baþladýðýnda rengi Data'dan alýp uygula
            if (m_ItemData != null && m_Renderer != null)
            {
                m_Renderer.material.color = m_ItemData.TintColor;
            }
        }

        public override bool Interact(GameObject interactor)
        {
            if (m_ItemData == null) return false;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddKey(m_ItemData);
                gameObject.SetActive(false);
                return true;
            }
            return false;
        }
    }
}