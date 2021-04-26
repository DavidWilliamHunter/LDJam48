using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam48
{
    public class Ball : MonoBehaviour
    {
        Rigidbody rb;
        float maxSpeed = 6.0f;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (rb.velocity.y > maxSpeed)
            {
                var vel = rb.velocity;
                vel.y = maxSpeed;
                rb.velocity = vel;
            } else if(rb.velocity.magnitude<0.005f)
            {
                StartCoroutine("checkDestroy");
            }
            if (transform.position.y < LevelController.GetLevelController().worldBottom.y)
                Destroy(gameObject);
        }

        IEnumerable checkDestroy()
        {
            yield return new WaitForSeconds(3);
            if (rb.velocity.magnitude < 0.005)
                Destroy(gameObject);
            yield return null;
        }

        public void destroyThis()
        {
            StartCoroutine("destroyThisImpl");
        }


        public IEnumerator destroyThisImpl()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer)
                renderer.enabled = false;
            var collider = GetComponent<Collider>();
            if (collider)
                collider.enabled = false;
            rb.velocity = Vector3.zero;
            ParticleSystem ps = GetComponentInChildren < ParticleSystem>();
            if (ps)
                ps.Play();
            yield return new WaitForSeconds(1f);
            Destroy(gameObject, 0.1f);
            yield return null;
        }
    }
}