using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.resources;
using RPG.combat;
namespace RPG.skill
{
    public class TrackArrows : MonoBehaviour
    {
        [SerializeField] Projectile projectile;
        [SerializeField] float maxSpawnRadius = 7f;
        [SerializeField] float minSpawnRadius = 3f;
        [SerializeField] float damage = 10f;
        Fighter fighter;
        Health target;
        private void Awake()
        {
            fighter = GetComponent<Fighter>();
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GenerateNails());
        }

        // Update is called once per frame
        void Update()
        {
            
            
        }

        private IEnumerator GenerateNails()
        {
            Health boss = GetComponent<Health>();
            int nailCount = 0;
            while (!boss.IsDead())
            {
                
                while (target == null) {
                    yield return null;
                    target = fighter.GetTarget();
                }
                
              //  print("Boss fire");
                
                Vector2 randomPos = Random.insideUnitCircle;
                Vector2 randomSpawnPosition = randomPos.normalized * (minSpawnRadius + randomPos.magnitude * maxSpawnRadius);
                Vector3 nailPosition = new Vector3(target.transform.position.x + randomSpawnPosition.x,
                    target.transform.position.y , target.transform.position.z + randomSpawnPosition.y);
                Projectile projectileInstance = Instantiate(projectile, nailPosition, Quaternion.identity);
                projectileInstance.SetTarget(target, gameObject, damage);
                nailCount++;
               // print("Nail count is " + nailCount);
                if (nailCount >= 7)
                {
                    nailCount = 0;
                    yield return new WaitForSeconds(10f);
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
                
            }
            
           
        }
    }
}

