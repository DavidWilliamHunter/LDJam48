using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    [CreateAssetMenu(fileName = "LevelDetails", menuName = "LDJAM/LevelDetails", order = 1)]
    public class LevelDetails : ScriptableObject
    {
        public float bumperStrength = 1.0f;

        public Transform doorPrefab;
        public int[] doorColumns;

    }
}