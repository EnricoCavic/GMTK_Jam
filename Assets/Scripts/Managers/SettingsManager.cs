using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private int frameRate = 60;

#if UNITY_EDITOR
        [SerializeField] private float timeScale = 1;
        private float lastFrametimeScale;
#endif
        private void Awake()
        {
            Application.targetFrameRate = frameRate;

#if UNITY_EDITOR
            Time.timeScale = timeScale;
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