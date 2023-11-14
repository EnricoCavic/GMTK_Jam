using UnityEngine;

[RequireComponent(typeof(LoadNextScene))]
public class FinishLevelTrigger : MonoBehaviour
{
    private LoadNextScene nextScene;

    private void Awake()
    {
        nextScene = GetComponent<LoadNextScene>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            nextScene.Load();
        }
    }

}
