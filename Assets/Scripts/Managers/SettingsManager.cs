using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private int frameRate = 60;
        [SerializeField] private float timeScale = 1;

        private void Awake()
        {
            Application.targetFrameRate = frameRate;
            Time.timeScale = timeScale;
        }
    }
}