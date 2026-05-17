using System;
using System.Collections;
using BattleBase.UI.PopUps;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleBase.SceneLoadingService
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        [SerializeField] private PopUp _fader;
        private bool _isLoading;

        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        private void Awake()
        {
            _fader.Init();
            _fader.HideInstantly();
        }

        public void Load(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (_isLoading)
                return;

            StartCoroutine(LoadSceneRoutine(name));
        }

        public void ReloadCurrentScene()
        {
            if (_isLoading)
                return;

            int currentScene = SceneManager.GetActiveScene().buildIndex;
            string name = GetSceneNameByIndex(currentScene);
            Load(name);
        }

        private string GetSceneNameByIndex(int buildIndex)
        {
            if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
                throw new ArgumentOutOfRangeException(nameof(buildIndex));

            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            return System.IO.Path.GetFileNameWithoutExtension(scenePath);
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            _isLoading = true;
            bool waitFading = true;
            _fader.Show(() => waitFading = false);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (waitFading || async.progress < 0.9f)
                yield return null;

            async.allowSceneActivation = true;
            waitFading = true;
            _fader.Hide(() => waitFading = false);

            while (waitFading)
                yield return null;

            _isLoading = false;
        }
    }
}