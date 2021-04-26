using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LDJam48
{
    public class OpenDoor : MonoBehaviour
    {
        HingeJoint[] joints;

        private void Start()
        {
            joints = GetComponentsInChildren<HingeJoint>();
        }

        public void Close()
        {
            foreach(var hinge in joints)
            {
                hinge.useSpring = true;
                var oldData = hinge.spring;
                oldData.targetPosition = 0;
                hinge.spring = oldData;
            }
        }

        public void Open()
        {
            foreach (var hinge in joints)
            {
                hinge.useSpring = true;
                var oldData = hinge.spring;
                oldData.targetPosition = -90;
                hinge.spring = oldData;
            }
        }

    }
}