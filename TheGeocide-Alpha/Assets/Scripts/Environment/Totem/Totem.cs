using Assets.Data.Dialogs.Definition;
using Assets.Scripts.NPC;
using Assets.Scripts.Ui.Upgrades;
using System;
using System.Collections;
using UnityEngine;

public class Totem : MonoBehaviour
{

    private AudioController _audioController;
    public GameObject TotemGfx;
    public GameObject BurriedIntObj;
    public GodType PortalTargetedLocation;

    [SerializeField]
    private DialogManagerChannel m_DialogueManagerChannel;

    [SerializeField]
    private DialogueSO DialogueUnlock;
    [SerializeField]
    private DialogAgent DialogueAgent;
    private DialogueState _dialogState;
    private string _dialogRecipient;

    [SerializeField]
    private UI_Upgrade_Panel _Panel;

    private Animator _TotemAnimator;
    private Interactable _interactable;
    private CircleCollider2D _circleCollider2D;


    private void Awake()
    {
        m_DialogueManagerChannel.OnDialogueEnd += OnDialogueEnd;
        m_DialogueManagerChannel.OnRequestDialogStateCompleted += OnRequestDialogStateCompleted;

        _dialogRecipient = DialogueAgent.GetRecipientName();
    }

    private void OnDestroy()
    {
        m_DialogueManagerChannel.OnDialogueEnd -= OnDialogueEnd;
        m_DialogueManagerChannel.OnRequestDialogStateCompleted -= OnRequestDialogStateCompleted;
    }

    // Start is called before the first frame update
    void Start()
    {
        _TotemAnimator = TotemGfx.GetComponent<Animator>();
        _audioController = GetComponent<AudioController>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _interactable = GetComponent<Interactable>();

        StartCoroutine(waitForState());
    }

    private IEnumerator waitForState()
    {
        var _nbAsk = 0;
        while (_dialogState == null || _nbAsk > 3)
        {
            yield return new WaitForSeconds(0.5f);
            m_DialogueManagerChannel.RaiseRequestDialogState(DialogueUnlock.GetID(), _dialogRecipient);
            _nbAsk++;
        }
    }

    private void OnRequestDialogStateCompleted(DialogueState dialogState)
    {
        if(dialogState.DialogId != DialogueUnlock.GetID() || _dialogState != null)
        {
            return;
        }

        _dialogState = dialogState;
        if (dialogState.IsRead)
        {
            BurriedIntObj.SetActive(false);
            _TotemAnimator.Play("totem_raise");
            _interactable.enabled = true;
            _circleCollider2D.enabled = true;
        }
        else
        {
            _interactable.enabled = false;
            _circleCollider2D.enabled = false;
        }
    }

    private bool IsMessageForOtherAgent(DialogueEvent dialog)
    {
        return dialog == null || dialog.GetID() != DialogueUnlock.GetID();
    }

    private void OnDialogueEnd(DialogueEvent dialog)
    {

        if (DialogueUnlock == null)
        {
            Debug.LogWarning($"{GetType().FullName} : No unlock target set for this totem: " + name);
            return;
        }

        if (IsMessageForOtherAgent(dialog))
        {
            return;
        }

        RaiseTotem();
    }

    private void RaiseTotem()
    {
        BurriedIntObj.SetActive(false);
        _audioController.Play("totem_raise");
        _TotemAnimator.Play("totem_raise");
    }

    public void OnTotemRaiseCompleted()
    {
       
        _circleCollider2D.enabled = true;
        _interactable.enabled = true;
    }

    public void OpenGui()
    {
        _Panel.onEnter();
    }
}
