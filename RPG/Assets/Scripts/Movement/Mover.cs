using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.core;
using RPG.resources;
namespace RPG.movement
{   [System.Serializable]
    public class Mover : MonoBehaviour,IAction,ISaveable
    {
        [SerializeField] Transform target;
        // Start is called before the first frame update
        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        private void Start()
        {
            
        }
        // Update is called once per frame
        void Update()
        {

            if (health.IsDead())
            {
               
                navMeshAgent.enabled = false;
            }

            UpdateAnimation();

        }
       
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        
        }
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
           
            MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
        private void UpdateAnimation()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);

        }

        [System.Serializable]
        struct MoverSaveData {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);


            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
      
            navMeshAgent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles= data.rotation.ToVector();
            navMeshAgent.enabled = true;
        }
    }

}
