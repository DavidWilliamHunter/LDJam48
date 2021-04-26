using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace LDJam48
{
    public class GenerateBuilderButtons : MonoBehaviour
    {
        // the tiles to display (and the order).
        public TileDictionary tileDictionary;

        public TileMap currentTileMap;
        public TileMapRender mapRender;

        public Transform UIPrefab;


        private void Start()
        {
            Assert.IsNotNull(tileDictionary);

            int[] enabled = new int[3];
            int j = 0;
            for (int i = 2; i < tileDictionary.prefabs.Length; ++i)
                enabled[j++] = i;

            BuildPanel(enabled);

        }

        protected void BuildPanel(int [] enabled)
        {
            foreach (int i in enabled)
            {
                var newPanel = Instantiate(UIPrefab, transform);
                UnityEngine.UI.Image image = newPanel.GetComponent<UnityEngine.UI.Image>();
                Rect rect = image.rectTransform.rect;
                Vector2 pos = image.rectTransform.anchoredPosition;
                pos.x = (i+3) * rect.width + (rect.width/2);
                image.rectTransform.anchoredPosition = pos; 

                UnityEngine.UI.Text labelObject;

                labelObject = newPanel.GetComponentInChildren<UnityEngine.UI.Text>();
                var imageObjects = newPanel.GetComponentsInChildren<UnityEngine.UI.Image>();
                foreach(var j in imageObjects)
                    j.sprite = tileDictionary.sprites[i];

                labelObject.text = tileDictionary.names[i];
                

                UnityEngine.UI.Button button = newPanel.GetComponent<UnityEngine.UI.Button>();
                button.onClick.AddListener(() =>{ BuildIconClicked(i); });
            }
        }


        protected void BuildIconClicked(int i)
        {
            Debug.Log(string.Format("Build Icon Clicked i:{0}", i));
            mapRender.currentTile = i;
        }

        public void QuitToMenu()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}