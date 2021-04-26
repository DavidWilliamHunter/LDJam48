using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    public class MusicScoreRenderer : MonoBehaviour
    {
        public MusicScore musicScore;

        public List<string> score;

        public Grid grid;
        protected List<Transform> gameObjects = new List<Transform>();
        public int TotalBeats = 0;


        public void Start()
        {
            LevelController.GetLevelController().gameSpaceWidth = musicScore.gameSpaceWidth;
            LevelController.GetLevelController().verticalSpace = musicScore.VerticalSpace;
            
            //grid = GetComponent<Grid>();
        }

        public void buildBells(TileMap tileMap)
        {
            int BeatNumber = 0;

            string layoutRawText = musicScore.BellsLayout.text;

            string [] lines = layoutRawText.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    if (line[0] == 'C')    // this line contains the guide letter so ignore it.
                    { }
                    else
                    if (line[0] == '-') // this line contains the bar, we will use this but ignore it for now.
                    {
                        if (musicScore.BarPrefab)
                            CreateObject(musicScore.BarPrefab, tileMap, BeatNumber, 0);
                    } else
                    {
                        for(int i = 0; i<line.Length; ++i)
                        {
                            switch (line[i])
                            {
                                case 'B': // make a bell
                                    CreateObject(musicScore.BellPrefab, tileMap, BeatNumber, i, true);
                                    break;
                                case '+':  // this the extention symbol, we don't use this atm.
                                    break;
                                case '#': // this is a rock
                                    CreateObject(musicScore.RockPrefab, tileMap, BeatNumber, i);
                                    break;
                                case '.': // this the symbol for empty space
                                default:
                                    break;
                            }
                        }
                        score.Add(line);
                        BeatNumber++;
                    }
                }
            }

            TotalBeats = BeatNumber;
            LevelController.GetLevelController().gameSpaceWidth = musicScore.gameSpaceWidth;
            LevelController.GetLevelController().verticalSpace = musicScore.VerticalSpace;
            LevelController.GetLevelController().TotalBeats = TotalBeats;
            LevelController.GetLevelController().worldBottom = grid.CellToWorld(new Vector3Int(0, -TotalBeats* musicScore.VerticalSpace, 0));
            Debug.Log("LevelController.MusicScoreRenderer: Setup complete");
            Debug.Log(LevelController.GetLevelController().TotalBeats);
            Debug.Log(LevelController.GetLevelController().worldBottom);
        }

        protected Transform CreateObject(Transform prototype, TileMap tileMap, int beatNo, int horizontalSpace, bool colour = false)
        {
            int halfWidth = musicScore.gameSpaceWidth / 2;
            Vector3Int position = new Vector3Int(horizontalSpace - halfWidth,-beatNo*musicScore.VerticalSpace, 0);
            Vector3 position3 = grid.CellToWorld(position);

            var obj = Instantiate(prototype, position3, transform.rotation, transform);
            gameObjects.Add(obj);

            tileMap.Add(new Vector2Int(position.x, position.y),0);

            Bell bell = obj.GetComponent<Bell>();
            if(bell)
            {
                bell.beat = beatNo;
                bell.note = horizontalSpace;
            }

            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.clip = musicScore.GetAudioClipByName(musicScore.melodyTrackName);
            }

            if (colour)
                if (musicScore.musicMaterial)
                {
                    var renderer = obj.GetComponent<Renderer>();
                    renderer.material = musicScore.musicMaterial.GetMaterial(horizontalSpace);
                }
            return obj;
        }

        public bool IsValidPlay(int beatNo, int noteNo)
        {
            return score[beatNo][noteNo] != 'B';
        }

        public bool IsHeld(int beatNo, int noteNo)
        {
            return score[beatNo][noteNo] == '+';
        }

        public bool AreAllHeldOrEmpty(int beatNo)
        {
            bool ret = false;
            for (int i = 0; i < score[beatNo].Length; ++i)
                if (!(score[beatNo][i] == '.' || score[beatNo][i] == '+'))
                    ret = true;
            return !ret;
        }

    }
}