using UnityEngine;

namespace DPA.Managers.Sound
{

    public interface IClipHolder
    {
        public AudioClip GetNextClip();
    }
}