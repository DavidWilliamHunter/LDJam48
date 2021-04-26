using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LDJam48
{
    public class FollowBall : MonoBehaviour
    {
        public Transform followObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var p = transform.position;
            //p.y -= 5.0f * Time.deltaTime;
            double pos = MusicController.GetController().positionInSong / MusicController.GetController().secondsPerBeat;

            p.y = -(float)pos * 2.0f;
            transform.position = p;
        }
    }
}