using UnityEngine;
using InteractionSystem.Runtime.Core;

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
            // Call base to log the interaction
            base.Interact(interactor);

            // TODO: In Phase 4.2, we will add this key to the Player's Inventory here.
            Debug.Log($"Picked up key: {m_KeyID}");

            // Disable or destroy the object to simulate pickup
            gameObject.SetActive(false);

            return true;
        }

        #endregion
    }
}