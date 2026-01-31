using System;
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    public class Inventory : MonoBehaviour
    {
        private List<ItemData> m_CollectedItems = new List<ItemData>();

        public static event Action<ItemData> OnItemAdded;

        public void AddKey(ItemData itemData)
        {
            if (itemData == null) return;

            bool alreadyHas = false;
            foreach (var item in m_CollectedItems)
            {
                if (item.ID == itemData.ID) alreadyHas = true;
            }

            if (!alreadyHas)
            {
                m_CollectedItems.Add(itemData);
                Debug.Log($"<color=yellow>Inventory:</color> Added {itemData.DisplayName}");

                OnItemAdded?.Invoke(itemData);
            }
        }

        public bool HasKey(string keyID)
        {
            foreach (var item in m_CollectedItems)
            {
                if (item.ID == keyID) return true;
            }
            return false;
        }
    }
}