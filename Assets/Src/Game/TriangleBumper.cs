using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    public class TriangleBumper : MonoBehaviour
    {
        //public Transform bumperSegment;
        private AudioSource audioSource;
        private Rigidbody rb;
        public float force;

        public Transform[] cornerBones;
        private Vector3[] boneImpulse = new Vector3[3];
        private Rigidbody[] boneRB = new Rigidbody[3];

        // Start is called before the first frame update
        void Start()
        {
            force = LevelController.GetLevelController().levelDetails.bumperStrength;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();

            for (int i = 0; i < 3; ++i)
            {
                boneImpulse[i] = cornerBones[i].position - transform.position;
                boneImpulse[i].Normalize();
                boneRB[i] = cornerBones[i].GetComponent<Rigidbody>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            Ball ballObj = collision.gameObject.GetComponent<Ball>();
            if (ballObj)
            {
                /*(
                Vector3 dir = collision.gameObject.transform.position - transform.position;
                dir.Normalize();
                ballRB.AddForce(dir * force, ForceMode.Impulse);
                //rb.AddForce(-dir, ForceMode.Impulse);*/
                Vector3 localForceDirection = Vector3.zero;
                int bone0=0, bone1=0;
                switch(impactDirection(collision.gameObject.transform.position))
                {
                    case 0:
                        localForceDirection = new Vector3(0.707f, 0.707f, 0.0f);
                        bone0 = 0;
                        bone1 = 2;
                        break;
                    case 1:
                        localForceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
                        bone0 = 0;
                        bone1 = 1;
                        break;
                    case 2:
                        localForceDirection = new Vector3(0.0f, -1.0f, 0.0f);
                        bone0 = 1;
                        bone1 = 2;
                        break;
                }
                Debug.Log(impactDirection(collision.gameObject.transform.position));
                Rigidbody ballRB = ballObj.GetComponent<Rigidbody>();
                ballRB.AddForce(transform.TransformDirection(localForceDirection) * force, ForceMode.Impulse);
                //ballRB.AddForce(localForceDirection * force, ForceMode.Impulse);
                boneRB[bone0].AddForce(boneImpulse[bone0], ForceMode.Impulse);
                boneRB[bone1].AddForce(boneImpulse[bone1], ForceMode.Impulse);
                audioSource.Play();
            }
        }

        // so the big side has values like 112 and 99.
        // the bottom side has values like 137, 178, 171
        // the big side has DP angles like -0.38, -0.156 etc.
        // the bottom side has values close to -1.
        // the right side has values close to 0.
        //        0                     1
        //      \                     \
        //      ¦\                    ¦\
        //   -1 ¦ \    +1          0  ¦ \    0
        //      ¦  \                  ¦  \
        //      *---*                 *---*
        //        0  \                  -1 \


        private int impactDirection(Vector3 impactWorldPos)
        {
            Vector3 localPos = transform.InverseTransformPoint(impactWorldPos)* 10000.0f;
            Debug.Log(impactWorldPos);
            Debug.Log(localPos*10000);
            if (localPos.x < -50.0f)
                return 1;
            if (localPos.y < -300.0f)
                return 2;
            return 0;
        }

        // (-79.4, -101.0, 14.0) - top
        // (101.5, -314.1, 14.0)  - bottom
        // (149.7, -36.2, 14.0)
        // (149.7, -36.2, 14.0)  - side
        // (224.9, -108.8, 14.0)  -side

        /*private int impactDirection(Vector3 impactWorldPos)
        {
            Vector3 worldDir = impactWorldPos - transform.position;
            worldDir.Normalize();
            Vector3 localDir = transform.InverseTransformDirection(worldDir);
            Debug.Log(localDir);
            Vector2 local2DDir = new Vector2(localDir.y, localDir.z);
            local2DDir.Normalize();
            float angle1 = Vector2.Dot(local2DDir, Vector2.up);
            float angle2 = Vector2.Dot(local2DDir, Vector2.left);
            Debug.Log("angle1:" + angle1);
            Debug.Log("angle2:" + angle2);
            if (angle2 < 0.0f && angle1 < 0.707f && angle1 >-0.707f)
                return 1;
            if (angle1 < 0.0f && angle2 < 0.707f && angle2 > -0.707f)
                return 2;
            else
                return 0;
        }*/
    }
}

// angle1 -0.948752  (wrong)
// angle2 -0.3131709

//angle1 -0.7752985
//angle2 0.622352

