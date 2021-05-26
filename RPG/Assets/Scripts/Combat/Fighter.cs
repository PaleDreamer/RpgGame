using System.Collections;
using System.Collections.Generic;
using UnityEngine;                 
using RPG.movement;
using RPG.core;
using RPG.Saving;
using RPG.resources;
using RPG.stats;
using GameDevTV.Utils;
namespace RPG.combat
{
    public class Fighter : MonoBehaviour,IAction,ISaveable,IModifierProvider
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Transform leftHand= null;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Weapon defaultWeapon = null;
     
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon = null;
        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }
        private void Start()
        {
            currentWeapon.ForceInit();
           
        }
        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
     
        }
        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(leftHand, rightHand, animator);
        }
        public Health GetTarget()
        {
            return target;
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead() == true) { return; }
            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
            if (!isInRange)
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
                //        Debug.Log("distance equals to" + Vector3.Distance(transform.position, target.position));
                //    Debug.Log("hitting move");
            }
            else
            {
                GetComponent<Mover>().Cancel();
                //       Debug.Log("distance equals to" + Vector3.Distance(transform.position, target.position));
                //   Debug.Log("attacking");
                AttackBehaviour();
            }
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
            
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //that will  trigger Hit event
                TriggerAttack();
                timeSinceLastAttack = 0;

            }
            
        }
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }
        //Animation Event
        void Hit()
        {
            if (target == null) { return ; }
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
  
            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(leftHand, rightHand, target,gameObject, damage);
            }
            else
            {
                target.takeDamage(gameObject, damage);
            }
            
            
        }
        void Shoot()
        {
            
            Hit();
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
            
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

     
    }

}
