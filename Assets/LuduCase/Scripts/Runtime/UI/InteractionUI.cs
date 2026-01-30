using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using InteractionSystem.Runtime.Core;   // For ItemData
using InteractionSystem.Runtime.Player; // For Inventory Events

namespace InteractionSystem.Runtime.UI
{
    public class InteractionUI : MonoBehaviour
    {
        #region Fields

        [Header("Interaction Prompt")]
        [Tooltip(" The panel containing the interaction text.")]
        [SerializeField] private GameObject m_PromptPanel;

        [Tooltip("The text component displaying 'Press E to Interact'.")]
        [SerializeField] private TextMeshProUGUI m_PromptText;

        [Tooltip("The slider used for Hold interactions.")]
        [SerializeField] private Slider m_ProgressBar;

        [Header("Inventory UI")]
        [Tooltip("The layout group where icons will be spawned.")]
        [SerializeField] private Transform m_InventoryContainer;

        [Tooltip("The prefab for the inventory item icon (Image component).")]
        [SerializeField] private GameObject m_ItemIconPrefab;

        [Header("Feedback Messages")]
        [Tooltip("Text for warnings like 'Locked' or 'Too Far'.")]
        [SerializeField] private TextMeshProUGUI m_FeedbackText;

        [Header("Settings")]
        [SerializeField] private Color m_NormalColor = Color.white;
        [SerializeField] private Color m_WarningColor = Color.red;

        // Coroutine reference to handle overlapping messages
        private Coroutine m_FeedbackRoutine;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            // SUBSCRIBE to the Inventory event
            // When an item is added, our AddNewItemIcon function will be called automatically.
            Inventory.OnItemAdded += AddNewItemIcon;
        }

        private void OnDisable()
        {
            // UNSUBSCRIBE to prevent errors if this object is destroyed
            Inventory.OnItemAdded -= AddNewItemIcon;
        }

        #endregion

        #region Inventory Methods

        // Triggered by the Event
        private void AddNewItemIcon(ItemData item)
        {
            if (item == null || m_ItemIconPrefab == null || m_InventoryContainer == null) return;

            GameObject newIconObj = Instantiate(m_ItemIconPrefab, m_InventoryContainer);

            Image img = newIconObj.GetComponent<Image>();
            if (img != null)
            {
                // Resim varsa koy (Yoksa Unity'nin varsayýlan beyaz karesi kalýr)
                if (item.Icon != null) img.sprite = item.Icon;

                // --- BURASI EKSÝK OLABÝLÝR: RENGÝ DEÐÝÞTÝR ---
                // Data'daki rengi (Kýrmýzý/Mavi) resmin rengine ata.
                img.color = item.TintColor;
            }

            StartCoroutine(AnimateIconPop(newIconObj.transform));
        }

        // Procedural Animation (Elastic Bounce) without using external assets
        private IEnumerator AnimateIconPop(Transform iconTransform)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            // Start invisible
            iconTransform.localScale = Vector3.zero;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Elastic Ease Out Formula
                // Creates a bouncy effect that overshoots 1.0 and settles back
                float scale = Mathf.Sin(-13.0f * (t + 1.0f) * Mathf.PI * 0.5f) * Mathf.Pow(2.0f, -10.0f * t) + 1.0f;

                iconTransform.localScale = Vector3.one * scale;
                yield return null;
            }

            // Ensure exact final scale
            iconTransform.localScale = Vector3.one;
        }

        #endregion

        #region Interaction Methods

        public void ShowPrompt(string message, bool isWarning = false)
        {
            if (m_PromptPanel != null) m_PromptPanel.SetActive(true);

            if (m_PromptText != null)
            {
                m_PromptText.text = message;
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
                // Show slider only if progress is started (> 0)
                m_ProgressBar.gameObject.SetActive(progress > 0);
                m_ProgressBar.value = progress;
            }
        }

        #endregion

        #region Feedback Methods

        public void ShowFeedbackMessage(string message, float duration)
        {
            if (m_FeedbackText != null)
            {
                // Stop previous message if it's still showing
                if (m_FeedbackRoutine != null) StopCoroutine(m_FeedbackRoutine);

                m_FeedbackRoutine = StartCoroutine(FeedbackRoutine(message, duration));
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

        #endregion
    }
}