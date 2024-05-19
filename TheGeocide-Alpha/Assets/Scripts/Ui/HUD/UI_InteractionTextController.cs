using UnityEngine;
using TMPro;

public class UI_InteractionTextController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Panel;
    [SerializeField]
    private TextMeshProUGUI m_TextField;

    [SerializeField]
    private FlowChannel _flowChannel;

    [SerializeField]
    private InteractionDetector m_WatchedInteractionInstigator;

    private bool _currentState = false;
    private bool _isInteractable = true;

    private void Awake()
    {
        m_Panel.SetActive(false);
        _flowChannel.OnFlowStateChanged += OnFlowStateChanged;
        _flowChannel.OnInteractionChanged += OnInteractionChanged;
    }

    private void OnDestroy()
    {
        _flowChannel.OnFlowStateChanged -= OnFlowStateChanged;
        _flowChannel.OnInteractionChanged -= OnInteractionChanged;
    }

    void Update()
    {
        var state = m_WatchedInteractionInstigator.enabled && m_WatchedInteractionInstigator.HasNearbyInteractables() && _isInteractable;

        if (_currentState != state)
        {
            if (m_WatchedInteractionInstigator.HasNearbyInteractables())
            {
                    UpdateInteractionGUI();
            }

            _currentState = state;
            m_Panel.SetActive(_currentState);
        }    
    }

    private void OnFlowStateChanged(FlowState state)
    {
        _isInteractable = state.IsInteractionPossible;
    }
    private void OnInteractionChanged()
    {
        if (m_WatchedInteractionInstigator.HasNearbyInteractables())
        {
            UpdateInteractionGUI();
        }
    }

    private void UpdateInteractionGUI()
    {
        m_TextField.SetText(LocalizationService.GetLocalizedString(m_WatchedInteractionInstigator.GetTextKey(), m_WatchedInteractionInstigator.GetTextTable()));
        Canvas.ForceUpdateCanvases();
    }
}
