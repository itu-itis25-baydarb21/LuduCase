using UnityEngine;
using TMPro;
using UnityEngine.UI;

// IMPORTANT: This namespace line must match what InteractionDetector is looking for
namespace InteractionSystem.Runtime.UI
{
    public class InteractionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject m_PromptPanel;
        [SerializeField] private TextMeshProUGUI m_PromptText;
        [SerializeField] private Slider m_ProgressBar;

        public void ShowPrompt(string message)
        {
            if (m_PromptPanel != null)
            {
                m_PromptPanel.SetActive(true);
                if (m_PromptText != null) m_PromptText.text = message;
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
    }
}