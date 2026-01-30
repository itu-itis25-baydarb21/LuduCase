using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// IMPORTANT: This namespace line must match what InteractionDetector is looking for
namespace InteractionSystem.Runtime.UI
{
    public class InteractionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject m_PromptPanel;
        [SerializeField] private TextMeshProUGUI m_PromptText;
        [SerializeField] private Slider m_ProgressBar;
        [SerializeField] private TextMeshProUGUI m_InventoryText; // Drag a new text here

        [Header("Colors")]
        [SerializeField] private Color m_NormalColor = Color.white;
        [SerializeField] private Color m_WarningColor = Color.red;

        [Header("Feedback UI")]
        [SerializeField] private TextMeshProUGUI m_FeedbackText; // Link the new text here

        public void ShowPrompt(string message, bool isWarning = false)
        {
            if (m_PromptPanel != null) m_PromptPanel.SetActive(true);

            if (m_PromptText != null)
            {
                m_PromptText.text = message;
                // Change color based on distance status
                m_PromptText.color = isWarning ? m_WarningColor : m_NormalColor;
            }
        }

        public void HidePrompt()
        {
            if (m_PromptPanel != null) m_PromptPanel.SetActive(false);
        }

        public void UpdateProgress(float progress)
        {
            if (m_ProgressBar != null)
            {
                m_ProgressBar.gameObject.SetActive(progress > 0);
                m_ProgressBar.value = progress;
            }
        }

        public void ShowFeedbackMessage(string message, float duration)
        {
            if (m_FeedbackText != null)
            {
                StopAllCoroutines(); // Reset if already showing a message
                StartCoroutine(FeedbackRoutine(message, duration));
            }
        }

        private IEnumerator FeedbackRoutine(string message, float duration)
        {
            m_FeedbackText.text = message;
            m_FeedbackText.gameObject.SetActive(true);

            yield return new WaitForSeconds(duration);

            m_FeedbackText.text = "";
            m_FeedbackText.gameObject.SetActive(false);
        }

        public void UpdateInventoryText(System.Collections.Generic.List<string> keys)
        {
            if (m_InventoryText != null)
            {
                if (keys.Count > 0)
                {
                    // Joins the list into a string: "Keys: RedKey, BlueKey"
                    m_InventoryText.text = "Keys: " + string.Join(", ", keys);
                }
                else
                {
                    m_InventoryText.text = ""; // Hide if empty
                }
            }
        }


    }
}