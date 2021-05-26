using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetDestroy = null;

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<ParticleSystem>().IsAlive() == false)
            {
                if (targetDestroy != null)
                {
                    Destroy(targetDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }
}

