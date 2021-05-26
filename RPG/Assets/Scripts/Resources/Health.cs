using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.core;
using RPG.stats;
using GameDevTV.Utils;
namespace RPG.resources
{
    
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;


        LazyValue<float>  health ;
        bool isDead = false;
        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start()
        {
            
          //  if (health<0)
          //  {
          //      health = GetComponent<BaseStats>().GetStat(Stat.Health);
          //  }
            
        }
        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }
        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health)*(regenerationPercentage/100);
            health.value = Mathf.Max(health.value, regenHealthPoints);
        }
        public float GetPercentage()
        {
            return 100* health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
   
        }
        public float GetHealthPoints()
        {
            return health.value;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public object CaptureState()
        {
            return health.value;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void RestoreState(object state)
        {
            float newHealth = (float)state ;
            health.value = newHealth;

            if (health.value == 0)
            {
                Die();
            }
       
        }

        public void takeDamage(GameObject instigator,float damage)
        {
            print(gameObject.name + " took damage: " + damage);
            health.value = Mathf.Max(health.value - damage, 0);
            if (health.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            float exp = GetComponent<BaseStats>().GetStat(Stat.Experience);
            print("exp="+exp);
            instigator.GetComponent<Experience>().GainExperience(exp);
        }
        private void Die()
        {
            if (isDead) { return; }
            isDead = true;
           
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

    }
}

