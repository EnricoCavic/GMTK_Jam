using System;
using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers.Sound
{
    [Serializable]
    public class RandomClip : IClipHolder
    {

        [SerializeField] private List<AudioClip> possibleClips;
        public AudioClip GetNextClip()
        {
            return possibleClips[UnityEngine.Random.Range(0, possibleClips.Count)];
        }
    }
}