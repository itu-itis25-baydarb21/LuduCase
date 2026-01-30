using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    // Adds this option to the Right Click -> Create menu in Unity
    [CreateAssetMenu(fileName = "NewItem", menuName = "InteractionSystem/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Data")]
        [Tooltip("Unique identifier for code logic (e.g., RedKey)")]
        public string ID;

        [Tooltip("Name displayed to the player in UI")]
        public string DisplayName;

        [Tooltip("Optional: Icon for UI inventory")]
        public Sprite Icon;
    }
}