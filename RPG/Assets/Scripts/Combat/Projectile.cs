using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.resources;
namespace RPG.combat
{
    public class Projectile : MonoBehaviour
    {
        
        [SerializeField] float speed = 5f;
        [SerializeField] bool homing = true;
        [SerializeField] bool isYieldTrack = false;
        [SerializeField] GameObject hitEffect=null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float maxLastTime = 10f;
        [SerializeField] float lifeAfterImpact = 0.2f;
        float damage = 50f;
        float rotateTime = 0.5f;
        Health target;
        GameObject instigator = null;
        // Update is called once per frame
        private void Start()
        {
            StartCoroutine(ArrowFly());
        }
        void Update()
        {
        
            //           moveDistance+= Vector3.forward * speed * Time.deltaTime;

        }

        IEnumerator ArrowFly()
        {
            if (isYieldTrack)
            {
               yield return StartCoroutine(RotateArrow(rotateTime));
            }
            else
            {
                transform.LookAt(GetAimLocation());
            }
            yield return MoveForward();
        }
        IEnumerator RotateArrow(float time)
        {
            Vector3 oldTargetLocation=GetAimLocation();
            transform.LookAt(oldTargetLocation);
            print("Get old location");
            yield return new WaitForSeconds(0.5f);
            Vector3 newTargetLocation = GetAimLocation();
            print("Get new location");
            Vector3 diffAim = newTargetLocation - oldTargetLocation;
            float i = 0;
            float rate = 1 / time;
            while (i < 1)
            {
                i += Time.deltaTime* rate;
                transform.LookAt(oldTargetLocation + diffAim * i);
                yield return null;
            }
            
        }

        IEnumerator MoveForward()
        {
            while (target == null) {
                yield return null;
            }

            while (true)
            {
                if (homing && !target.IsDead())
                {

                transform.LookAt(GetAimLocation());
                }

            
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                yield return null;
            }
            
        }
        public void SetTarget(Health target,GameObject instigator,float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy(gameObject, maxLastTime);
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + targetCapsule.height / 2 * Vector3.up;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;

            if (target.IsDead())
            {
                
                return;
            }
            speed = 0;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach(GameObject objectToDelete in destroyOnHit)
            {
                Destroy(objectToDelete);
            }

                target.takeDamage(instigator,damage);
                Destroy(gameObject, lifeAfterImpact);
            
           
        }
    }
}

