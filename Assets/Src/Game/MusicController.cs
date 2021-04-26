using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDJam48
{
    public class MusicController : MonoBehaviour
    {
        static MusicController _musicController;

        static public MusicController GetController()
        {
            if(!_musicController)
            {
                _musicController = FindObjectOfType<MusicController>();
            }
            return _musicController;
        }
        // Store audio sources
        //AudioSource[] audioSources;
        //Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
        public AudioSource bassAudioSource;
        public Transform audioPrefab;

        public MusicScore musicScore;


        public int songPosition = 0;

        #region MetronomeVariables
        public bool Metronome = false;
        public AudioSource metronomeAudioSource;
        #endregion

        public double bpm = 160.0f;

        public double startTime;
        public double startDelay = 1.0f;
        public double lastBeat;
        public double lastBeatPlayed;
        public double timeSinceSongStart;
        public double secondsPerBeat;

        public double positionInSong;
        public int currentSongBeat;
        public int roundedSongBeat;

        private double positionInBeat;
        private int beatNumber;

        public void Start()
        {
            //SetupAudioTrackes(audioClipNames, audioClips);
            SetupAudioTracks();
            StartSong();
        }

        public void Update()
        {
            double time = AudioSettings.dspTime;
            timeSinceSongStart = time - startTime;
            beatNumber = (int) System.Math.Floor(timeSinceSongStart / secondsPerBeat);
            lastBeat = ((float)beatNumber) * secondsPerBeat;
            positionInBeat = (timeSinceSongStart - lastBeat) / secondsPerBeat;

            //AudioSource melody = musicScore.GetAudioClipByName(musicScore.melodyTrackName);
            positionInSong = bassAudioSource.time;
            currentSongBeat = (int)System.Math.Floor(positionInSong / secondsPerBeat);
            roundedSongBeat = (int)System.Math.Round(positionInSong / secondsPerBeat);

            if (lastBeat > lastBeatPlayed)
            {
                if(Metronome)
                    metronomeAudioSource.Play();

                lastBeatPlayed = lastBeat;
            }

            if (currentSongBeat > songPosition)    // if we havn't hit a bell and so can't advance.
            {
                //foreach (var audioSource in audioSources)
                //audioSource.Value.Pause();
                bassAudioSource.Pause();
            } else
            if(currentSongBeat <= songPosition)  // simply play the song for another beat
            {
                //foreach (var audioSource in audioSources)
                //   audioSource.Value.UnPause();
                bassAudioSource.UnPause();
            } else                                  // the song is now behind and must be forcably advanced.
            {
                //foreach (var audioSource in audioSources)
                //    audioSource.Value.time = (float) (((double) currentSongBeat ) / bpm);
                //bassAudioSource.time = (float)(((double)songPosition) * secondsPerBeat);
            } 
        }

        public void StartSong()
        {
            Debug.Log("StartSong");
            startTime = AudioSettings.dspTime + startDelay;
            secondsPerBeat = 60.0f / (float) bpm;
            songPosition = 0;
            Debug.Log("startTime" + startTime);
            Debug.Log("secondsPerBeat" + secondsPerBeat);
            Debug.Log("songPosition" + songPosition);

            // start audio
            //AudioSource melody = audioSources[melodyTrackName];
            //melody.PlayScheduled(startTime);
            //AudioSource bass = audioSources[bassTrackName];
            bassAudioSource.PlayScheduled(startTime);
        }



        /*private void SetupAudioSources(IEnumerable<string> names, IEnumerable<AudioClip> audioClips)
        {
            IEnumerator<AudioClip> audio = audioClips.GetEnumerator();
            IEnumerator<string> name = names.GetEnumerator();
            while(audio.MoveNext() && name.MoveNext())
            {
                Transform obj = Instantiate(audioPrefab, transform);
                AudioSource audioSource = obj.GetComponent<AudioSource>();
                audioSource.clip = audio.Current;
                audioSources.Add(name.Current, audioSource);
            }
        } */

        /*private void SetupAudioTrackes(IEnumerable<string> names, IEnumerable<AudioClip> audioClips)
        {
            IEnumerator<AudioClip> audio = audioClips.GetEnumerator();
            IEnumerator<string> name = names.GetEnumerator();
            while(audio.MoveNext() && name.MoveNext())
            {
                Transform obj = Instantiate(audioPrefab, transform);
                AudioSource audioSource = obj.GetComponent<AudioSource>();
                audioSource.clip = audio.Current;
                Debug.Log(audioSource);
                Debug.Log(name);
                Debug.Log(name.Current);
                audioSources.Add(name.Current, audioSource);
            }
        } */
        private void SetupAudioTracks()
        {
            //Transform obj = Instantiate(audioPrefab, transform);
            //bassAudioSource = obj.GetComponent<AudioSource>();
            bassAudioSource.clip = musicScore.GetAudioClipByName(musicScore.bassTrackName);
        }

        private void DestroyAudioSources()
        {
            /*if (audioSources != null)
            {
                foreach (var audioSource in audioSources)
                    Destroy(audioSource.Value.gameObject);
                audioSources.Clear();
            }*/
        }

        public void AdvanceOneNote()
        {
            songPosition++;
        }

    }
}