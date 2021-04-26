using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDJam48
{
    public class LevelController : MonoBehaviour
    {
        public LevelDetails levelDetails;
        public MusicScoreRenderer musicScoreRenderer;
        public TileMapRender tileMapRender;

        public int gameSpaceWidth;
        public int TotalBeats;
        public int verticalSpace;
        public Vector3 worldTop = Vector3.zero;
        public Vector3 worldBottom;



        static public LevelController _levelController;

        static public LevelController GetLevelController()
        {
            if (_levelController)
                return _levelController;
            else
            {
                _levelController = FindObjectOfType<LevelController>();
                return _levelController;
            }
        }

        public void Start()
        {
            Debug.Log("PerfromLevelStartUpOperations");

            StartCoroutine("PerfromLevelStartUpOperations");
        }

        IEnumerator PerfromLevelStartUpOperations()
        {
            Debug.Log("musicScoreRenderer");
            musicScoreRenderer = FindObjectOfType<MusicScoreRenderer>();
            Debug.Log("tileMapRender");
            TileMapRender tileMapRender = FindObjectOfType<TileMapRender>();
            Assert.IsNotNull(musicScoreRenderer);
            if(!musicScoreRenderer.enabled)
                yield return null;
            musicScoreRenderer.buildBells(tileMapRender.tileMap);

            Assert.IsNotNull(tileMapRender);
            if (!tileMapRender.enabled)
                yield return null;
            Debug.Log("tileMapRender");
            tileMapRender.UpdateTileMap(true);
            yield return null;

            foreach(int col in levelDetails.doorColumns)
            {
                Vector3 pos = tileMapRender.grid.CellToWorld(new Vector3Int(-tileMapRender.emptyWidth + col, 2, 0));
                pos.x += 0.1f;
                var obj = Instantiate(levelDetails.doorPrefab, pos, levelDetails.doorPrefab.rotation, transform);
                OpenDoor door = obj.GetComponent<OpenDoor>();
                //door.Close();
                BallSpawner.GetBallSpawner().spawnLocations.Add(door.transform);
                BallSpawner.GetBallSpawner().doors.Add(door);
            }
            LightProbes.TetrahedralizeAsync();

            yield return null;

        }

        public void BellHit(Bell bell)
        {
            int currentSongBeat = MusicController.GetController().roundedSongBeat;
            //if(musicScoreRenderer.IsValidPlay(currentSongBeat, bell.note))   // if this is a valid note for this time.
            //{
            double currentTime = AudioSettings.dspTime - MusicController.GetController().startTime;
            double currentTimeInBeats = currentTime / MusicController.GetController().secondsPerBeat;
            double nextBeatStartTime = System.Math.Round(currentTimeInBeats) * MusicController.GetController().secondsPerBeat + MusicController.GetController().startTime;
            int noteLength = 1;
            while (musicScoreRenderer.AreAllHeldOrEmpty(MusicController.GetController().songPosition + noteLength))
                noteLength++;


            if (MusicController.GetController().songPosition < currentSongBeat)
            {
                bell.Play(currentSongBeat * MusicController.GetController().secondsPerBeat,
                    (currentSongBeat + noteLength) * MusicController.GetController().secondsPerBeat,
                    (float)nextBeatStartTime);
                MusicController.GetController().AdvanceOneNote();
                //while (musicScoreRenderer.AreAllHeldOrEmpty(MusicController.GetController().songPosition))
                //    MusicController.GetController().AdvanceOneNote();
            }
            //}
        }
    }
}