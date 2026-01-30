using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.UI;

namespace InteractionSystem.Runtime.Player
{
    public class Inventory : MonoBehaviour
    {
        // We keep storing IDs for simplicity in logic checks
        private List<string> m_CollectedKeyIDs = new List<string>();

        [SerializeField] private InteractionUI m_UI;

        // CHANGED: Now accepts ItemData parameter
        public void AddKey(ItemData itemData)
        {
            if (itemData == null) return;

            if (!m_CollectedKeyIDs.Contains(itemData.ID))
            {
                m_CollectedKeyIDs.Add(itemData.ID);
                Debug.Log($"<color=yellow>Inventory:</color> Added {itemData.DisplayName}");

                // Update the UI
                if (m_UI != null) m_UI.UpdateInventoryText(m_CollectedKeyIDs);
            }
        }

        public bool HasKey(string keyID)
        {
            return m_CollectedKeyIDs.Contains(keyID);
        }
    }
}