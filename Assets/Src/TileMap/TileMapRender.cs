using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDJam48
{
    public class TileMapRender : MonoBehaviour
    {
        public TileMap tileMap;
        public TileDictionary tileDictionary;
        public Transform rockPrefab;

        public int emptyWidth = 5;
        public int height = 100;
        public int fillToWidth = 15;

        public Grid grid;
        private Plane gridPlane;

        public bool EditMode = true;
        public int AddMouseButton = 0;
        public int RemoveMouseButton = 0;

        public int currentTile = 0;
        public int currentRot = 0;

        private Transform UIElement;
        public Transform UIElementPrefab;
        private Transform UIArrow;
        public Transform UIArrowPrefab;

        private Dictionary<Vector2Int, Transform> gameObjects;
        private Dictionary<Vector2Int, Transform> fillObjects;

        public void Start()
        {
            if (!grid)
                grid = GetComponent<Grid>();
            Assert.IsNotNull(grid);
            gridPlane = CalculateGridPlane();

            //UpdateTileMap(true);

            UIElement = Instantiate(UIElementPrefab, transform);
            UIElement.gameObject.SetActive(false);

            UIArrow = Instantiate(UIArrowPrefab, transform);
            UIArrow.gameObject.SetActive(false);


        }

        public void Update()
        {
            if (EditMode)
            {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (gridPlane.Raycast(mouseRay, out float enter))
                    {
                        Vector3 mousePos3D = mouseRay.GetPoint(enter);
                        Vector3Int gridPosition = grid.WorldToCell(mousePos3D);
                        if (gridPosition.x >= -emptyWidth && gridPosition.x <= emptyWidth)
                        {
                            if (Input.GetMouseButtonDown(AddMouseButton))
                            {

                                tileMap.Add(new Vector2Int(gridPosition.x, gridPosition.y), TileValueJoin(currentTile, currentRot));
                                UpdateTileMap();
                            }
                            else
                            if (Input.GetMouseButtonDown(RemoveMouseButton))
                            {
                                tileMap.Remove(new Vector2Int(gridPosition.x, gridPosition.y));
                                UpdateTileMap();
                            }
                            else
                            {
                                UIElement.transform.position = grid.CellToWorld(gridPosition);
                                UIElement.gameObject.SetActive(true);

                                Vector3 p = grid.CellToWorld(gridPosition);
                                p.z = -1.2f;
                                UIArrow.transform.position = p;

                                UIArrow.rotation = Quaternion.Euler(0.0f, 0.0f, currentRot * 90);
                                UIArrow.gameObject.SetActive(true);
                            }
                        } else
                        {
                            UIElement.gameObject.SetActive(false);
                            UIArrow.gameObject.SetActive(false);
                        }
                    }
                    if(Input.GetKeyDown("r")) // is R key pressed
                    {
                        currentRot++;
                        if (currentRot > 3)
                            currentRot = 0;
                    }
                } else
                {
                    UIElement.gameObject.SetActive(false);
                    UIArrow.gameObject.SetActive(false);

                }

            }
        }

        // compare the for collection of game objects to the tile map and alter where neccessary.
        public void UpdateTileMap(bool forceReload = false)
        {
            Debug.Log("TileMap.UpdateTileMap: Setup complete");

            emptyWidth = LevelController.GetLevelController().gameSpaceWidth/2;
            height = LevelController.GetLevelController().TotalBeats * LevelController.GetLevelController().verticalSpace;

            Debug.Log("emptyWidth:" + emptyWidth);
            Debug.Log("height:" + height);
            if (forceReload)
                deleteAllTiles();
            if (gameObjects == null)
                gameObjects = new Dictionary<Vector2Int, Transform>(tileMap.tiles.Count);

            // its not a good idea to delete items in the middle of iteration so keep a delete lsit
            List<Vector2Int> toDelete = new List<Vector2Int>();
            // foreach current gameObject check that there is supposed to be a tile there.
            foreach (var KeyValue in gameObjects)
            {
                if(!tileMap.tiles.ContainsKey(KeyValue.Key))
                {
                    toDelete.Add(KeyValue.Key);                    
                }
            }

            foreach(var Key in toDelete)
            {
                Destroy(gameObjects[Key]?.gameObject);  // destroy the game object in the current tile pos
                gameObjects.Remove(Key);   // remove the now destroyed record.
            }


            foreach (var KeyValue in tileMap.tiles)
            {
                if (!gameObjects.ContainsKey(KeyValue.Key))  // if the tile is not found in the tile map
                    createTileAt(KeyValue.Key, KeyValue.Value);
                //else
                    // TODO check that this is the correct tile type.
            }

            // now create the fill if it isn't already created.
            if (fillObjects == null)
                fillObjects = new Dictionary<Vector2Int, Transform>();
            if (fillObjects.Count == 0)
            {
                for (int j = 10; j > -height; --j)
                {
                    for (int i = -fillToWidth; i <= -emptyWidth; ++i)
                    {
                        Vector3 position = grid.CellToWorld(new Vector3Int(i, j, 0));
                        var obj = Instantiate(rockPrefab, position, transform.rotation, transform);
                        fillObjects.Add(new Vector2Int(i, j), obj);
                    }
                    for (int i = emptyWidth; i <= fillToWidth; ++i)
                    {
                        Vector3 position = grid.CellToWorld(new Vector3Int(i, j, 0));
                        var obj = Instantiate(rockPrefab, position, transform.rotation, transform);
                        fillObjects.Add(new Vector2Int(i, j), obj);
                    }
                }
            }
        }

        public void deleteAllTiles()
        {
            if (gameObjects!=null)
                foreach (var KeyValue in gameObjects)
                {
                    Destroy(KeyValue.Value?.gameObject);
                }
            gameObjects = new Dictionary<Vector2Int, Transform>();
            if (fillObjects != null)
                foreach (var KeyValue in fillObjects)
                {
                    Destroy(KeyValue.Value?.gameObject);
                }
            fillObjects = new Dictionary<Vector2Int, Transform>();
        }

        public void createTileAt(Vector2Int loc, int tile)
        {
            if (tile > 0) // use zero to hold blockers;
            {
                Vector3 position = grid.CellToWorld(new Vector3Int(loc.x, loc.y, 0));

                // tiles can be rotated so the last 4 values are the angles.
                TileValueSep(tile, out int tileNo, out int rot);
                Transform prefab = tileDictionary.prefabs[tileNo];
                Quaternion angle = transform.rotation;
                angle *= Quaternion.AngleAxis(90, Vector3.right);
                angle *= Quaternion.AngleAxis(rot * 90, Vector3.up);
                var obj = Instantiate(prefab, position, angle, transform); // create a new prefab object with this object as parent.
                gameObjects.Add(loc, obj);
            }
        }

        public void deleteTileAt(Vector2Int loc)
        {
            if (gameObjects.TryGetValue(loc, out Transform obj))
            {
                Destroy(obj?.gameObject);
                gameObjects.Remove(loc);
            }
        }

        private Plane CalculateGridPlane()
        {
            Vector3 a = grid.CellToWorld(new Vector3Int(0, 0, 0));
            Vector3 b = grid.CellToWorld(new Vector3Int(1, 0, 0));
            Vector3 c = grid.CellToWorld(new Vector3Int(0, 1, 0));
            return new Plane(a, b, c);
        }

        public void TileValueSep(int Value, out int tileNo, out int rot)
        {
            tileNo = Value / 4;
            rot = Value % 4;
        }

        public int TileValueJoin(int tileNo, int rot)
        {
            return tileNo * 4 + rot;
        }
    }
}