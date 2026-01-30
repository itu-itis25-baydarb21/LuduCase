using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;
using UnityEngine;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// A specific interactable representing a key that can be picked up.
    /// </summary>
    public class KeyItem : InstantInteractable
    {
        #region Fields

        [Tooltip("The unique ID or name of this key (e.g., 'RedKey').")]
        [SerializeField] private string m_KeyID = "GeneralKey";

        #endregion

        #region Properties

        public string KeyID => m_KeyID;

        #endregion

        #region Methods

        public override bool Interact(GameObject interactor)
        {
            base.Interact(interactor);

            // Try to find the Inventory on the player (interactor)
            Inventory inventory = interactor.GetComponent<Inventory>();

            if (inventory != null)
            {
                inventory.AddKey(m_KeyID);
                Debug.Log($"Picked up key: {m_KeyID}");

                // Disable object to verify pickup
                gameObject.SetActive(false);
                return true;
            }
            else
            {
                Debug.LogWarning("No Inventory component found on Interactor!");
                return false;
            }
        }

        #endregion
    }
}