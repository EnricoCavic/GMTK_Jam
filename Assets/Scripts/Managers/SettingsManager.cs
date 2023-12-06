using UnityEngine;

namespace DPA.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private int frameRate = 60;

        [SerializeField] private float timeScale = 1;

#if UNITY_EDITOR
        private float lastFrametimeScale;
#endif

        private void Awake()
        {
            Application.targetFrameRate = frameRate;
            Time.timeScale = timeScale;

#if UNITY_EDITOR
            lastFrametimeScale = timeScale;
#endif
        }

#if UNITY_EDITOR
        public void Update()
        {
            if(lastFrametimeScale == timeScale) return;
            Time.timeScale = timeScale;
            lastFrametimeScale = timeScale;
        }
#endif
    }
}