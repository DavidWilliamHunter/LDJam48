using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LDJam48
{
    public class CameraController : MonoBehaviour
    {
        LevelController levelController;
        public float MoveSpeed = 50.0f;

        public void Start()
        {
            levelController = LevelController.GetLevelController();
        }

        private void Update()
        {
            float upDown = Input.GetAxis("Vertical");

            Vector3 position = transform.position;
            position = new Vector3(position.x, position.y += upDown*MoveSpeed * Time.deltaTime, position.z);

            if (position.y > levelController.worldTop.y)
                position.y = levelController.worldTop.y;
            else if(position.y < levelController.worldBottom.y)
                position.y = levelController.worldBottom.y;

            transform.position = position;
        }
    }

}