using System.Collections;
using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    // Changed to ToggleInteractable to support Open/Close states
    public class Chest : ToggleInteractable
    {
        #region Fields

        [Header("Visuals")]
        [SerializeField] private Transform m_LidVisual; // Drag the "Lid_Hinge" here
        [SerializeField] private Vector3 m_OpenRotation = new Vector3(-45, 0, 0); // Rotation when open
        [SerializeField] private Vector3 m_ClosedRotation = Vector3.zero; // Rotation when closed

        [Header("Animation")]
        [SerializeField] private float m_AnimationDuration = 1.0f;

        // Private Animation State
        private Coroutine m_CurrentRoutine;

        #endregion

        #region Methods

        // Override logic to handle specific Chest messages
        protected override void OnStateChanged(bool isOpen)
        {
            // 1. Play Animation
            if (m_CurrentRoutine != null) StopCoroutine(m_CurrentRoutine);

            Quaternion targetRot = Quaternion.Euler(isOpen ? m_OpenRotation : m_ClosedRotation);
            m_CurrentRoutine = StartCoroutine(AnimateLid(targetRot));

            // 2. Update Prompt
            if (isOpen)
            {
                Debug.Log("<color=gold>Chest Opened!</color>");
                m_InteractionPrompt = "Hold {KEY} to Close";
            }
            else
            {
                Debug.Log("Chest Closed.");
                m_InteractionPrompt = "Hold {KEY} to Open";
            }
        }

        private IEnumerator AnimateLid(Quaternion target)
        {
            Quaternion start = m_LidVisual.localRotation;
            float elapsed = 0f;

            while (elapsed < m_AnimationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / m_AnimationDuration;
                t = Mathf.SmoothStep(0f, 1f, t); // Smooth ease-in/out

                m_LidVisual.localRotation = Quaternion.Slerp(start, target, t);
                yield return null;
            }

            m_LidVisual.localRotation = target;
        }

        #endregion
    }
}