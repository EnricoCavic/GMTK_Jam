using DPA.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private AssetReference nextScene;
    public void Load() => SceneManager.Instance.LoadScene(nextScene);
}
