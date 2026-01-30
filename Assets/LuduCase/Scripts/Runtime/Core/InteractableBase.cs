using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] protected string m_InteractionPrompt = "Press {KEY} to Interact";
        [SerializeField] private float m_HoldDuration = 0f;

        // OUTLINE ÝÇÝN GEREKLÝLER
        [Header("Outline Settings")]
        // Inspector'dan oluþturduðun M_Outline materyalini buraya atacaksýn
        [SerializeField] private Material m_OutlineMaterial;

        // Objenin üzerindeki Mesh Renderer'larý bulacaðýz
        private Renderer[] m_Renderers;
        // Orijinal materyal listelerini hafýzada tutacaðýz
        private List<Material[]> m_OriginalMaterialLists = new List<Material[]>();

        public string InteractionPrompt => m_InteractionPrompt;
        public float HoldDuration => m_HoldDuration;

        protected virtual void Awake()
        {
            // Bu obje ve altýndaki tüm görsel parçalarý bul (Kapýnýn kolu, sandýðýn kapaðý vs.)
            m_Renderers = GetComponentsInChildren<Renderer>();

            // Orijinal hallerini kaydet (Outline'ý silerken lazým olacak)
            foreach (var renderer in m_Renderers)
            {
                m_OriginalMaterialLists.Add(renderer.sharedMaterials);
            }
        }

        public virtual bool Interact(GameObject interactor)
        {
            Debug.Log($"Interacting with: {gameObject.name}");
            return true;
        }

        // --- OUTLINE MANTIÐI ---

        public void OnFocus()
        {
            if (m_OutlineMaterial == null) return;

            for (int i = 0; i < m_Renderers.Length; i++)
            {
                var renderer = m_Renderers[i];
                var materials = new List<Material>(renderer.sharedMaterials);

                // Eðer listede zaten yoksa Outline'ý ekle
                if (!materials.Contains(m_OutlineMaterial))
                {
                    materials.Add(m_OutlineMaterial);
                    renderer.materials = materials.ToArray();
                }
            }
        }

        public void OnLoseFocus()
        {
            // Orijinal materyal listelerine geri dön
            for (int i = 0; i < m_Renderers.Length; i++)
            {
                if (m_Renderers[i] != null)
                {
                    m_Renderers[i].materials = m_OriginalMaterialLists[i];
                }
            }
        }
        public void RefreshMaterialBackup()
        {
            // Eðer rendererlar henüz bulunmadýysa bul
            if (m_Renderers == null || m_Renderers.Length == 0)
            {
                m_Renderers = GetComponentsInChildren<Renderer>();
            }

            // Eski listeyi temizle
            m_OriginalMaterialLists.Clear();

            // Þu anki (boyanmýþ) haliyle yeniden kaydet
            foreach (var renderer in m_Renderers)
            {
                // .sharedMaterials deðil .materials kullanýyoruz ki instance'ý alalým
                m_OriginalMaterialLists.Add(renderer.materials);
            }
        }

    }
}