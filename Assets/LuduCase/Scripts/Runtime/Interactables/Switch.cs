using UnityEngine;
using UnityEngine.Events;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// A physical switch that triggers UnityEvents when toggled.
    /// </summary>
    public class Switch : ToggleInteractable
    {
        #region Events

        [Header("Events")]
        [Tooltip("Event invoked when switch is turned ON.")]
        public UnityEvent OnSwitchActivate;

        [Tooltip("Event invoked when switch is turned OFF.")]
        public UnityEvent OnSwitchDeactivate;

        #endregion

        #region Methods

        protected override void OnStateChanged(bool newState)
        {
            // Visual feedback (Rotate the switch handle, change color, etc.)
            // For now, we just change the name/color or log
            Debug.Log($"Switch is now: {(newState ? "ON" : "OFF")}");

            if (newState)
            {
                OnSwitchActivate?.Invoke();
            }
            else
            {
                OnSwitchDeactivate?.Invoke();
            }
        }

        #endregion
    }
}