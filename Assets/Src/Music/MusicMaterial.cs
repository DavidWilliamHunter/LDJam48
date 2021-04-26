using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    [CreateAssetMenu(fileName = "MusicMaterial", menuName = "LDJAM/MusicMaterial", order = 1)]
    public class MusicMaterial : ScriptableObject
    {
        public Material[] materials;

        public Material GetMaterial(int number)
        {
            return materials[number % materials.Length];
        }
    }
}