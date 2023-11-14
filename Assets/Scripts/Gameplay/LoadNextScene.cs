using DPA.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private AssetReference nextScene;
    public void Load() => SceneHandler.Instance.LoadScene(nextScene);
}
