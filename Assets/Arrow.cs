using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{

    public class Arrow : MonoBehaviour
    {
        public float force;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay(Collider other)
        {
            Vector3 globalDirection = transform.TransformDirection(Vector3.back);
            Ball ball = other.gameObject.GetComponent<Ball>();
            if(ball)
            {
                var rb = ball.GetComponent<Rigidbody>();
                rb.velocity = globalDirection * force;
            }

        }
    }
}