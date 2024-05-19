using Assets.Scripts.GameManager;
using Assets.Scripts.Ui.Menu;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private GameSceneChannel _gmc;


    [SerializeField]
    private GuiNavigationChannel _navChannel;

    [SerializeField]
    private Button _optionButton;

    [SerializeField]
    private List<MenuView> _registeredView;

    [SerializeField]
    private bool _isDebug;

    private MenuView _currentView;

    private void Awake()
    {
        _navChannel.OnViewRequested += ChangeView;
    }

    private void OnDestroy()
    {
        _navChannel.OnViewRequested -= ChangeView;
    }

    void Start()
    {
        if (_isDebug)
        {
            return;
        }

        _currentView = _registeredView.First(item => item.Parent == null);

        foreach(var item in _registeredView)
        {
            item.SetActiveView(false);
        }

        _currentView.SetActiveView(true);
    }

    public void PlayGame()
    {
        _audioController.Play("Clic");
        _gmc.RaiseSceneRequest(SceneIndex.Hub);
    }

    public void ChangeView(string ViewName)
    {
        if (_currentView.GetName() == ViewName)
        {
            return;
        }

        _audioController.Play("Clic");
        _currentView.SetActiveView(false);
        _currentView = _registeredView.First(item => item.GetName() == ViewName);
        _currentView.SetActiveView(true);
    }

    public void BackClic()
    {
        _audioController.Play("Clic");
        if(_currentView.Parent == null)
        {
            return;
        }

        _currentView.SetActiveView(false);
        _currentView = _registeredView.First(item => item.GetName() == _currentView.GetParentName());
        _currentView.SetActiveView(true);
    }

    public void OnButtonHover()
    {
        _audioController.Play("Hover");
    }

    public void QuitGame()
    {
        _audioController.Play("Clic");
        Application.Quit();
    }
}
