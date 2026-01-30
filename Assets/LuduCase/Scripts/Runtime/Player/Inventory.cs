using System; // Action için gerekli
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.Core;
// InteractionSystem.Runtime.UI'ý sildik! UI'ý tanýmýyoruz artýk.

namespace InteractionSystem.Runtime.Player
{
    public class Inventory : MonoBehaviour
    {
        private List<ItemData> m_CollectedItems = new List<ItemData>();

        // EVENT TANIMI:
        // "Bir eþya eklendiðinde bu olayý dinleyenlere haber ver"
        // Action<ItemData> demek: Haber verirken yanýnda eklenen eþyayý da gönder demek.
        public static event Action<ItemData> OnItemAdded;

        public void AddKey(ItemData itemData)
        {
            if (itemData == null) return;

            // Zaten var mý kontrolü
            bool alreadyHas = false;
            foreach (var item in m_CollectedItems)
            {
                if (item.ID == itemData.ID) alreadyHas = true;
            }

            if (!alreadyHas)
            {
                m_CollectedItems.Add(itemData);
                Debug.Log($"<color=yellow>Inventory:</color> Added {itemData.DisplayName}");

                // UI'ý güncelle demiyoruz. Sadece "OLAY VAR!" diye baðýrýyoruz.
                // ?.Invoke þu demek: Eðer dinleyen biri varsa çalýþtýr.
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