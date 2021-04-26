using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LDJam48
{
    public class MenuUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TryIt()
        {
            SceneManager.LoadScene("TileMapTest", LoadSceneMode.Single);
        }

        public void JustPlay()
        {
            SceneManager.LoadScene("JustPlay", LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}