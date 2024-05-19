using Assets.Scripts.GameManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{

    public bool IsGamePaused = false;
    public bool IsPossibleToDisplay = false;

    public GameObject PauseMenuUi;
    public GameObject btn_resume;
    [SerializeField]
    private PlayerStateChannel _playerStateChannel;

    [SerializeField]
    private GameSceneChannel _gmc;

    private SceneIndex _currentScene;

    private void Awake()
    {
        _playerStateChannel.OnDeath += OnDeath;
        _gmc.OnSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(SceneIndex levelIndex)
    {
        _currentScene = levelIndex;
    }

    private void OnDestroy()
    {
        _playerStateChannel.OnDeath -= OnDeath;
        _gmc.OnSceneChanged -= OnSceneChanged;
    }

    private void OnDeath()
    {
        if(btn_resume == null)
        {
            return;
        }

        StartCoroutine(DisplayMenu());
    }

    private IEnumerator DisplayMenu()
    {
        yield return new WaitForSeconds(2);
        btn_resume.SetActive(false);
        Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.Find("Player").GetComponent<Player>();
        Time.timeScale = 1f;
        InputHandler.instance.OnEscape += OnEscape;
    }

    private void OnEscape(InputHandler.InputArgs obj)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (IsGamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        if(PauseMenuUi == null)
        {
            return;
        }
        PauseMenuUi.SetActive(true);       
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void Resume()
    {
        if (PauseMenuUi == null || btn_resume == null)
        {
            return;
        }

        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void Restart()
    {
        Resume();
        btn_resume.SetActive(true);
        _gmc.RaiseSceneRequest(_currentScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
