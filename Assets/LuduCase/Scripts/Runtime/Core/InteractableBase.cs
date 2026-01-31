using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] protected string m_InteractionPrompt = "Press {KEY} to Interact";
        [SerializeField] private float m_HoldDuration = 0f;

        [Header("Outline Settings")]
        [SerializeField] private Material m_OutlineMaterial;

        private Renderer[] m_Renderers;
        private List<Material[]> m_OriginalMaterialLists = new List<Material[]>();

        public string InteractionPrompt => m_InteractionPrompt;
        public float HoldDuration => m_HoldDuration;

        protected virtual void Awake()
        {
            m_Renderers = GetComponentsInChildren<Renderer>();

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

        public void OnFocus()
        {
            if (m_OutlineMaterial == null) return;

            for (int i = 0; i < m_Renderers.Length; i++)
            {
                var renderer = m_Renderers[i];
                var materials = new List<Material>(renderer.sharedMaterials);

                if (!materials.Contains(m_OutlineMaterial))
                {
                    materials.Add(m_OutlineMaterial);
                    renderer.materials = materials.ToArray();
                }
            }
        }

        public void OnLoseFocus()
        {
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
            if (m_Renderers == null || m_Renderers.Length == 0)
            {
                m_Renderers = GetComponentsInChildren<Renderer>();
            }

            m_OriginalMaterialLists.Clear();

            foreach (var renderer in m_Renderers)
            {
                m_OriginalMaterialLists.Add(renderer.materials);
            }
        }

    }
}