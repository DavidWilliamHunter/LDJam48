using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    public class Bell : MonoBehaviour
    {
        public int beat; // play on beat number
        public int note;
        AudioSource audioSource;
        public Material DarkBell;

        double audioStopTime = 0;
        bool isPlaying = false;
        bool hasPlayed = false;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            double time = AudioSettings.dspTime;
            if (isPlaying && time > audioStopTime)
            {
                audioSource.Stop();
                isPlaying = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasPlayed)
            {
                Ball ball = collision.gameObject.GetComponent<Ball>();
                if (ball)
                {
                    LevelController.GetLevelController().BellHit(this);
                    var rb = ball.GetComponent<Rigidbody>();
                    Vector3 dir = ball.transform.position - transform.position;
                    dir.Normalize();
                    rb.AddForce(dir * LevelController.GetLevelController().levelDetails.bumperStrength, ForceMode.Impulse);
                }
            }
        }

        public void InactivateWithoutPlaying()
        {
            isPlaying = true;
            hasPlayed = true;
            var renderer = GetComponent<Renderer>();
            renderer.material = DarkBell;
            var collider = GetComponent<Collider>();
            collider.enabled = false;
        }
        public void Play(double from, double to, float startTime)
        {
            if (!isPlaying && !hasPlayed)
            {
                /*Debug.Log(string.Format("From :{0}", from));
                Debug.Log(string.Format("To :{0}", to));
                Debug.Log(string.Format("startTime :{0}", startTime));
                Debug.Log(string.Format("currentTime :{0}", AudioSettings.dspTime));*/
                audioSource.PlayScheduled(startTime);
                audioSource.time = (float)from;
                audioStopTime = startTime + (to - from);
                isPlaying = true;
                hasPlayed = true;
                var renderer = GetComponent<Renderer>();
                renderer.material = DarkBell;
                var collider = GetComponent<Collider>();
                collider.enabled = false;
            }
        }
    }
}