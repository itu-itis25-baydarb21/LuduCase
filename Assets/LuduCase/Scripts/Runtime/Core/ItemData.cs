using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "InteractionSystem/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Data")]
        public string ID;
        public string DisplayName;
        public Sprite Icon;

        // YENÝ EKLENEN KISIM: Renk Seçimi
        [Header("Visuals")]
        public Color TintColor = Color.white;
    }
}