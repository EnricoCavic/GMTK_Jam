using DPA.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace DPA.Managers
{
    public class SceneHandler : Singleton<SceneHandler>
    {
        [SerializeField] private AssetReference firstScene;

#if UNITY_EDITOR
        [SerializeField] private bool testMode = false;
#endif
        public event Action onLoadSceneCompleted;
        private SceneInstance loadedScene;

        private void Awake()
        {
            if (!InstanceSetup(this)) return;

#if UNITY_EDITOR
            if (testMode)
                return;
#endif

            var handle = Addressables.LoadSceneAsync(firstScene, LoadSceneMode.Additive);
            handle.Completed += (operation) =>
            {
                loadedScene = new();
                loadedScene = operation.Result;
            };
        }

        void Start()
        {
#if UNITY_EDITOR
            if (!testMode) return;
            SceneManager.SetActiveScene(gameObject.scene);
#endif
        }

        public void LoadScene(object _sceneToLoad)
        {
            if (loadedScene.Scene.path == null)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                LoadAdditiveScene(_sceneToLoad);
                return;
            }

            var unloadHandle = Addressables.UnloadSceneAsync(loadedScene);
            unloadHandle.Completed += (operation) => LoadAdditiveScene(_sceneToLoad);
        }

        public void LoadAdditiveScene(object _sceneToLoad)
        {
            var loadHandle = Addressables.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
            loadHandle.Completed += (operation) =>
            {
                loadedScene = new();
                loadedScene = operation.Result;
                onLoadSceneCompleted?.Invoke();

            };
        }

    }
}