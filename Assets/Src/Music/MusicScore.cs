using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    [CreateAssetMenu(fileName = "MusicScore", menuName = "LDJAM/MusicScore", order = 1)]
    public class MusicScore : ScriptableObject
    {
        public MusicMaterial musicMaterial;

        public TextAsset BellsLayout;
        public int gameSpaceWidth = 14;
        public int VerticalSpace = 4;

        public Transform BellPrefab;
        public Transform BarPrefab;
        public Transform RockPrefab;

        public string melodyTrackName = "melody";
        public string bassTrackName = "bass";

        public AudioClip[] audioClips;
        public string[] audioClipNames;

        public AudioClip GetAudioClipByName(string name)
        {
            for(int i=0; i< audioClipNames.Length; ++i)
                if(audioClipNames[i]==name)
                {
                    return audioClips[i];
                }
            return null;
        }
    }
}