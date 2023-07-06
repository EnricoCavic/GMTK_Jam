using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers.Sound
{
    [CreateAssetMenu(fileName = "New Randomized Track", menuName = "Retro/Sound/Randomized Track")]
    public class RandomizedTrackSO : SoundtrackSO
    {
        [SerializeField] private List<RandomClip> clipTracks;

        public override List<IClipHolder> GetClipHolders()
        {
            List<IClipHolder> clipHolders = new();
            for (int i = 0; i < clipTracks.Count; i++)
            {
                clipHolders.Add(clipTracks[i]);
            }
            return clipHolders;
        }
    }
}