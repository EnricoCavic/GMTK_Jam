using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace DPA.Managers
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private AssetReference gameplayManagers;
        void Start()
        {
            var loadSceneHandle = Addressables.LoadSceneAsync(gameplayManagers, LoadSceneMode.Additive);
            loadSceneHandle.Completed += (handle) => UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}