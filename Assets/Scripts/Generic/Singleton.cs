using UnityEngine;

namespace DPA.Generic
{
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        public static T Instance;

        public bool InstanceSetup(T _instance)
        {
            if (Instance != null)
            {
                Destroy(_instance.gameObject);
                return false;
            }
            else
            {
                Instance = _instance;
                return true;
            }

        }
    }
}