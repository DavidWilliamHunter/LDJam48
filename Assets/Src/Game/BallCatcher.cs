using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LDJam48
{
    public class BallCatcher : MonoBehaviour
    {
        int count = 0;
        public int expected;
        public UnityEngine.Events.UnityEvent allBalls;

        private void OnTriggerEnter(Collider other)
        {
            Ball ballComponent = other.gameObject.GetComponent<Ball>();
            if(ballComponent)
            {
                count++;
                if (count >= expected)
                    allBalls.Invoke();
            }
        }

        public void reset()
        {
            count = 0;
        }
    }
}