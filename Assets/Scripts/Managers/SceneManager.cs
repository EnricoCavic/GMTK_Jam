using DPA.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace DPA.Managers
{
    public class SceneManager : Singleton<SceneManager>
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
            if (testMode) return;
#endif

            var handle = Addressables.LoadSceneAsync(firstScene, LoadSceneMode.Additive);
            handle.Completed += (operation) =>
            {
                loadedScene = new();
                loadedScene = operation.Result;
            };
        }

        public AsyncOperationHandle<SceneInstance> LoadScene(object _assetKey)
        {
            /*
            Addressables.UnloadSceneAsync(loadedScene);
            
            var handle = Addressables.LoadSceneAsync(_assetKey, LoadSceneMode.Additive);
            handle.Completed += (operation) =>
            {
                loadedScene = new();
                loadedScene = operation.Result;
                onLoadSceneCompleted?.Invoke();
            };
            */

            
            var unloadHandle = Addressables.UnloadSceneAsync(loadedScene);
            unloadHandle.Completed += (unloaded) =>
            {
                var handle = Addressables.LoadSceneAsync(_assetKey, LoadSceneMode.Additive);
                handle.Completed += (operation) =>
                {
                    loadedScene = new();
                    loadedScene = operation.Result;
                    onLoadSceneCompleted?.Invoke();

                };

            };

            return unloadHandle;
            
            // return handle;
        }

    }
}