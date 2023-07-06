using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers.Sound
{
    public abstract class SoundtrackSO : ScriptableObject
    {
        public float bpm = 140.0f;
        public int beatsPerSegment = 8;
        public abstract List<IClipHolder> GetClipHolders();
    }
}