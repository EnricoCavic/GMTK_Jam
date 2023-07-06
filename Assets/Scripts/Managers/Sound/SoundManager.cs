using DPA.Generic;
using DPA.Managers.Sound;
using System.Collections.Generic;
using UnityEngine;

namespace DPA.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private float preScheduleTime = 1.0f;

        public SoundtrackSO soundtrack;
        private List<IClipHolder> clipTracks;

        private const float MINUTE = 60.0f;
        private double nextEventTime;
        private int flip;
        private float trackVolume = 1f;

        // estruturar os audio sources como pares para ter mais controle deles
        private List<AudioSource> soundtrackChannels;

        private void Awake()
        {
            if (!InstanceSetup(this)) return;
            if (soundtrack == null) return;

            clipTracks = new(soundtrack.GetClipHolders());
  
            soundtrackChannels = new();
            GameObject soundChannel;
            int totalSoundtrackChannels = clipTracks.Count * 2;
            for (int i = 0; i < totalSoundtrackChannels; i++)
            {
                soundChannel = new GameObject("SoundChannel" + i);
                soundChannel.transform.parent = transform;
                var audioSource = soundChannel.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                soundtrackChannels.Add(audioSource);
            }

            trackVolume = 0f;
            UpdateVolume();

            nextEventTime = AudioSettings.dspTime + preScheduleTime;
        }

        private void Update()
        {
            if (soundtrack == null) return;

            double time = AudioSettings.dspTime;

            if (trackVolume < 1f)
            {
                trackVolume += Time.deltaTime / 10;
                UpdateVolume();
            }

            if (time + preScheduleTime > nextEventTime)
            {
                ScheduleChannels();
            }
        }

        private void ScheduleChannels()
        {
            AudioSource channel;
            int trackCount = clipTracks.Count;
            for (int i = 0; i < trackCount; i++)
            {
                channel = soundtrackChannels[i + flip];
                channel.clip = clipTracks[i].GetNextClip();
                channel.PlayScheduled(nextEventTime);
            }
            flip = trackCount - flip;

            nextEventTime += MINUTE / soundtrack.bpm * soundtrack.beatsPerSegment;
        }

        private void UpdateVolume()
        {
            for (int i = 0; i < soundtrackChannels.Count; i++)
            {
                soundtrackChannels[i].volume = trackVolume;
            }
        }
    }
}