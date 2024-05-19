using Assets.Scripts.Localization;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
    [SerializeField]
    private GameObject _mainPanel;

    private TextMeshProUGUI m_SpeakerText;
    private TextMeshProUGUI m_DialogueText;
    private Image m_SpeakerAvatar;

    private GameObject m_ChoicesBoxPanel;

    [SerializeField]
    private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private bool m_ListenToInput = false;
    private DialogueNode m_NextNode = null;

    private void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        m_DialogueChannel.OnDialogueStart += OnDialogueStart;
        m_DialogueChannel.OnDialogueEnd += OnDialogueEnd;



        m_SpeakerText = _mainPanel.transform.Find("HeaderPanel/Speaker").GetComponent<TextMeshProUGUI>();
        m_SpeakerAvatar = _mainPanel.transform.Find("HeaderPanel/Avatar").GetComponent<Image>();

        m_DialogueText = _mainPanel.transform.Find("ContentPanel/DialogText").GetComponent<TextMeshProUGUI>();      
        m_ChoicesBoxPanel = _mainPanel.transform.Find("ContentPanel/ChoicePanel").gameObject;

        _mainPanel.SetActive(false);
        m_ChoicesBoxPanel.SetActive(false);
    }


    private void Start()
    {
        InputHandler.instance.OnInteraction += OnInteraction;
    }

    private void OnInteraction(InputHandler.InputArgs obj)
    {
        if (m_ListenToInput)
        {
            m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
        }
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;

        m_DialogueChannel.OnDialogueStart -= OnDialogueStart;
        m_DialogueChannel.OnDialogueEnd -= OnDialogueEnd;

        InputHandler.instance.OnInteraction -= OnInteraction;
    }


    private void OnDialogueStart(DialogueSO dialogue)
    {
        _mainPanel.SetActive(true);
        StartCoroutine(EnableInputsListener());
    }

    private IEnumerator EnableInputsListener()
    {
        yield return new WaitForSeconds(1);
        m_ListenToInput = true;
    }

    private void OnDialogueNodeStart(DialogueNode node)
    {
        m_DialogueText.text = LocalizationService.GetLocalizedString(node.Text, TradTable.Dialog);
        m_SpeakerText.text = node.Speaker.Name;
        m_SpeakerAvatar.sprite = node.Speaker.Avatar;

        node.Accept(this);
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        m_NextNode = null;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxPanel.transform)
        {
            Destroy(child.gameObject);
        }

        
        m_ChoicesBoxPanel.SetActive(false);
    }

    private void OnDialogueEnd(DialogueSO dialogue)
    {
        m_ListenToInput = false;
        _mainPanel.SetActive(false);
    }

    public void Visit(TextDialogueNode node)
    {
        m_NextNode = node.NextNode;
    }

    public void Visit(ChoiceDialogueNode node)
    {
        m_ChoicesBoxPanel.SetActive(true);

        foreach (DialogueChoice choice in node.Choices)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxPanel.GetComponent<RectTransform>());
            newChoice.Choice = choice;
        }
    }
}