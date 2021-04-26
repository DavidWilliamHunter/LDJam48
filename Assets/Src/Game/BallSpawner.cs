using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    public class BallSpawner : MonoBehaviour
    {
        public static BallSpawner _BallSpawner;
        public static BallSpawner GetBallSpawner()
        {
            if(!_BallSpawner)
            {
                _BallSpawner = FindObjectOfType<BallSpawner>();
            }
            return _BallSpawner;
        }

        public Transform ballPrefab;
        public List<Transform> spawnLocations;
        public List<OpenDoor> doors;

        public bool spawnInfinite = true;
        public int noToSpawn = 5;
        public float timeBetweenSpawn = 1.0f;

        private float lastSpawn;
        private int spawned;
        private int spawner;

        private bool doorOpen;
        // Start is called before the first frame update
        void Start()
        {

            reset();
            if (doors == null)
                doors = new List<OpenDoor>(spawnLocations.Count);
            int i = 0;
            foreach (Transform spawnLoc in spawnLocations)
                doors[i++] = spawnLoc.GetComponent<OpenDoor>();

        }

        // Update is called once per frame
        void Update()
        {
            if (spawned < noToSpawn || spawnInfinite)
            {
                if (!doorOpen)
                {
                    foreach (var door in doors)
                        door.Open();
                    doorOpen = true;
                }
                if (Time.fixedTime > (lastSpawn + timeBetweenSpawn))
                {
                    Instantiate(ballPrefab, spawnLocations[spawner].position, spawnLocations[spawner].rotation, transform);
                    lastSpawn = Time.fixedTime;
                    spawned++;
                    spawner = spawner == (spawnLocations.Count - 1) ? 0 : spawner + 1;
                }
            }
            else
            {
                if (doorOpen)
                {
                    foreach (var door in doors)
                        door.Close();
                    doorOpen = false;
                }
            }
        }

        public void reset()
        {
            lastSpawn = Time.fixedTime;
            spawned = 0;
            spawner = 0;
        }
    }
}