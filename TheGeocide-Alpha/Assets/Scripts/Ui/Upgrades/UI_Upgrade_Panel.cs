using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Ui.Upgrades
{
    public abstract class UI_Upgrade_Panel : MonoBehaviour
    {

        [SerializeField]
        private GameObject UpgradePanel;
        [SerializeField]
        private FlowChannel m_GameFlowChannel;
        [SerializeField]
        private FlowState m_InGameFlowState;
        [SerializeField]
        private FlowState m_InGuiFlowState;
        [SerializeField]
        private AudioChannel m_AudioChannel;

        private void Start()
        {
            InputHandler.instance.OnEscape += OnEscape;
        }

        private void OnEscape(InputHandler.InputArgs obj)
        {
            if (!UpgradePanel.activeSelf)
            {
                return;
            }
            onExit();
        }

        public void onEnter()
        {
            m_GameFlowChannel.RaiseFlowStateChanged(m_InGuiFlowState);
            UpgradePanel.SetActive(true);
        }

        public void onExit()
        {
            m_GameFlowChannel.RaiseFlowStateChanged(m_InGameFlowState);
            m_AudioChannel.RaiseAudioRequest(new AudioEvent("exit"));
            UpgradePanel.SetActive(false);
        }
    }
}