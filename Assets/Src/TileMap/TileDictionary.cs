using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    [CreateAssetMenu(fileName = "TileMap", menuName = "LDJAM/TileDictionary", order = 1)]
    public class TileDictionary : ScriptableObject
    {
        public Transform[] prefabs;
        public Sprite[] sprites;
        public string[] names;
    }
}