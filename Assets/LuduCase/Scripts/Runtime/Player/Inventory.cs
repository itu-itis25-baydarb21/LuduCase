using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Manages the player's collected items (specifically keys).
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        #region Fields

        // We use a List to store the IDs of collected keys
        [SerializeField] private List<string> m_CollectedKeys = new List<string>();
        [SerializeField] private InteractionSystem.Runtime.UI.InteractionUI m_UI;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a key to the inventory.
        /// </summary>
        /// <param name="keyID">The unique ID of the key.</param>
        public void AddKey(string keyID)
        {
            if (!m_CollectedKeys.Contains(keyID))
            {
                m_CollectedKeys.Add(keyID);
                Debug.Log($"<color=yellow>Inventory:</color> Added Key '{keyID}'");

                // NEW: Update the visual list
                if (m_UI != null)
                {
                    m_UI.UpdateInventoryText(m_CollectedKeys);
                }
            }
        }

        /// <summary>
        /// Checks if the player possesses a specific key.
        /// </summary>
        /// <param name="keyID">The ID to check for.</param>
        /// <returns>True if the key is in inventory.</returns>
        public bool HasKey(string keyID)
        {
            return m_CollectedKeys.Contains(keyID);
        }


        #endregion
    }
}