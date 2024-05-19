using Assets.Scripts.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Choice;
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueNode m_ChoiceNextNode;

    public DialogueChoice Choice
    {
        set
        {
            //Debug.Log($"Debug: {value.ChoicePreview}");
            m_Choice.text = LocalizationService.GetLocalizedString(value.ChoicePreview, TradTable.Dialog);
            m_ChoiceNextNode = value.ChoiceNode;
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        m_DialogueChannel.RaiseRequestDialogueNode(m_ChoiceNextNode);
    }
}