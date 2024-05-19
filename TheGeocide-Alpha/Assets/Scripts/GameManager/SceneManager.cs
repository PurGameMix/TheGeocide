using Assets.Data.Common;
using Assets.Scripts.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManager
{
    public class SceneManager : MonoBehaviour
    {

        public static SceneManager instance;
        public GameObject _loadingScreen;
        public Camera loadCam;
        public TextMeshProUGUI loreText;
        public CanvasGroup alphaCanvas;
        public TextListSO loreTradKeyList;

        [SerializeField]
        private AudioListener _rootAudioListener;

        private float  _loadingTimer;
        private bool _isLoading = false;
        private int _minLoadingScreenSecond = 3;

        private SceneIndex _currentScene = SceneIndex.Root;
        [SerializeField]
        private GameSceneChannel _gmc;
        private void Awake()
        {
            instance = this;

            _gmc.OnSceneRequested += OnSceneRequested;
            _gmc.OnCurrentSceneRequested += OnCurrentSceneRequested;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnCurrentSceneRequested()
        {
            _gmc.RaiseCurrentSceneResponse(_currentScene);
        }

        private void OnDestroy()
        {
            _gmc.OnSceneRequested -= OnSceneRequested;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            #if !DEBUG
            LoadScene(SceneIndex.TitleScreen);
            #endif
        }



        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("Scene loaded: " + scene.isLoaded);

            var sceneIndex = (SceneIndex) scene.buildIndex;
            if (sceneIndex.IsRoot())
            {
                return;
            }

            loadCam.enabled = false;
            _loadingScreen.gameObject.SetActive(false);
            _rootAudioListener.enabled = false;

            _currentScene = sceneIndex;
            _isLoading = false;
            _gmc.RaiseSceneChanged(sceneIndex);
        }

        private void _HandleLoadingScreenDisplayOn()
        {
            loadCam.enabled = true;
            _rootAudioListener.enabled = false;
            _loadingScreen.gameObject.SetActive(true);

            StartCoroutine(TimeLoading());
            StartCoroutine(GenerateLoreText());
        }

        private IEnumerator GenerateLoreText()
        {
            var currentLore = new List<string>(loreTradKeyList.TradKeyList);

            _DisplayText(currentLore);

            while (_isLoading)
            {                           
                yield return new WaitForSeconds(6f);
                LeanTween.alphaCanvas(alphaCanvas, 0, 1f);
                yield return new WaitForSeconds(0.5f);
                _DisplayText(currentLore);
                LeanTween.alphaCanvas(alphaCanvas, 1, 1f);
            }
        }

        private void _DisplayText(List<string> currentLore)
        {
            var loreKeyIndex = UnityEngine.Random.Range(0, currentLore.Count);
            var tradKey = currentLore[loreKeyIndex];
            currentLore.Remove(tradKey);
            loreText.text = LocalizationService.GetLocalizedString(tradKey, TradTable.GUI);
        }

        private void OnSceneRequested(SceneIndex levelIndex)
        {
            Debug.Log("LoadingScene");
            LoadScene(levelIndex);
        }

        public void LoadScene(SceneIndex sceneToLoad)
        {
            _HandleLoadingScreenDisplayOn();

            if(!_currentScene.IsRoot())
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)_currentScene);

            }

            var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive);
            StartCoroutine(_HandleLoadingScreenDisplayDelay(asyncLoad));
        }

        private IEnumerator DelayActivation(float seconds, AsyncOperation asyncLoad)
        {
            yield return new WaitForSeconds(seconds);
            asyncLoad.allowSceneActivation = true;
        }

        private IEnumerator _HandleLoadingScreenDisplayDelay(AsyncOperation asyncLoad)
        {
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {

                // Check if the load has finished
                if (asyncLoad.progress >= 0.9f)
                {
                    //If loading to fast, wait

                    Debug.Log($"_loadingTimer : {_loadingTimer}, _minLoadingScreenSecond: {_minLoadingScreenSecond}");
                    if (_loadingTimer > _minLoadingScreenSecond)
                    {
                        Debug.Log($"passed");
                        asyncLoad.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }

        private IEnumerator TimeLoading()
        {
            _isLoading = true;
            _loadingTimer = 0;

            while (_isLoading)
            {
                _loadingTimer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
