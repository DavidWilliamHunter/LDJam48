using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    [CreateAssetMenu(fileName = "TileMap", menuName = "LDJAM/TileMap", order = 1)]
    public class TileMap : ScriptableObject
    {
        public TileDictionary tilePrefabs;

        public SerializableDictionary<Vector2Int, int> tiles;

        public void Add(Vector2Int loc, int tile)
        {
            tiles[loc] = tile;
        }

        public void Remove(Vector2Int loc)
        {
            if(tiles.ContainsKey(loc))
                tiles.Remove(loc);
        }
    }
}