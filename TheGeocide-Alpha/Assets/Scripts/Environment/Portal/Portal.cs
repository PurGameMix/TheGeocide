using Assets.Data.Dialogs.Definition;
using Assets.Scripts.GameManager;
using UnityEngine;

public class Portal : MonoBehaviour, IPortal
{
    [SerializeField]
    private AudioController _audioController;
    [SerializeField]
    private Animator _portalAnimator;
    [SerializeField]
    private Collider2D _interactCollider;

    public SceneIndex PortalTargetedLocation;

    [SerializeField]
    private DialogManagerChannel m_DialogueManagerChannel;
    [SerializeField]
    private GameSceneChannel m_GameManagerChannel;

    [SerializeField]
    private DialogueSO DialogueUnlock;

    private void Awake()
    {
        m_DialogueManagerChannel.OnDialogueEnd += OnDialogueEnd;
        _interactCollider.enabled = false;
    }

    private void OnDestroy()
    {
        m_DialogueManagerChannel.OnDialogueEnd -= OnDialogueEnd;
    }

    private void OnDialogueEnd(DialogueEvent dialog)
    {

        if(DialogueUnlock == null)
        {
            Debug.LogWarning($"{GetType().FullName} : No unlock target set for this portal: " + name);
            return;
        }

        if(dialog == null || dialog.GetID() != DialogueUnlock.GetID())
        {
            return;
        }

        OpenPortal();
    }

    public void OpenPortal()
    {
        _portalAnimator.Play("Portal_Open");
        _audioController.Play("portal_open");
    }

    public void OnPortalOpeningCompleted()
    {
        _portalAnimator.Play("Portal_Idle");
        _audioController.Stop("portal_open");
        _audioController.Play("portal_idle");
        _interactCollider.enabled = true;
    }

    public void GoToTarget()
    {
        _audioController.Play("portal_enter");

        //Debug.Log("GoToTarget");
        m_GameManagerChannel.RaiseSceneRequest(PortalTargetedLocation);
    }
}
