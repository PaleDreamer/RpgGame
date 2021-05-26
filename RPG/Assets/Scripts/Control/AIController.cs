using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.combat;
using RPG.resources;
using RPG.movement;
using RPG.core;
using GameDevTV.Utils;
namespace RPG.control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 55f;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance=1f;
        [SerializeField] float waypointDwellTime = 3f;
        int currentWaypointIndex = 0;
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();

            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }
        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }
        private void Start()
        {

            guardPosition.ForceInit();
        }
        // Update is called once per frame
        void Update()
        {

            if (health.IsDead()) return;

            if (InAttackRange() && fighter.CanAttack(player))
            {
                
                AttackBehaviour();

            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();

            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }
        private void SuspicionBehaviour() 
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;
            if (AtWaypoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition);
            }
            
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        private bool InAttackRange()
        {
            //          Debug.Log("Distance = "+Vector3.Distance(player.transform.position, transform.position));
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}
