using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{

    public class DestroyBall : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball)
                ball.destroyThis();
        }
    }
}