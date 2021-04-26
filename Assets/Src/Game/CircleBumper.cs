using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    public class CircleBumper : MonoBehaviour
    {
        //public Transform bumperSegment;
        private AudioSource audioSource;
        private Rigidbody rb;
        public float force;

        // Start is called before the first frame update
        void Start()
        {
            force = LevelController.GetLevelController().levelDetails.bumperStrength;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            Ball ballObj = collision.gameObject.GetComponent<Ball>();
            if(ballObj)
            {
                Rigidbody  ballRB = ballObj.GetComponent<Rigidbody>();
                Vector3 dir = collision.gameObject.transform.position - transform.position;
                dir.Normalize();
                ballRB.AddForce(dir * force, ForceMode.Impulse);
                rb.AddForce(-dir, ForceMode.Impulse);
                audioSource.Play();
            }
        }
    }
}