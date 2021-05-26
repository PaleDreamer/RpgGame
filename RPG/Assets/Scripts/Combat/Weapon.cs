using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.resources;
namespace RPG.combat
{
    [CreateAssetMenu(fileName = "Weapon",menuName ="Weapon/Make New Weapon",order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "weapon";
        public void Spawn(Transform leftHand,Transform rightHand, Animator animator)
        {
            DestroyOldWeapon(leftHand, rightHand);
            if (equippedPrefab != null)
            {
                GameObject weapon= Instantiate(equippedPrefab, GetWeaponTransform( leftHand, rightHand));
                weapon.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController;
            }
            
           
        }
        private void DestroyOldWeapon(Transform leftHand, Transform rightHand)
        {
            Transform oldWeapon= rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;
            oldWeapon.name = "Destroy";
            Destroy(oldWeapon.gameObject);
        }
        public void LaunchProjectile(Transform leftHand, Transform rightHand,Health target,GameObject instigator,float calculatedDamage)
        {
         
                Projectile projectileInstance = Instantiate(projectile, GetWeaponTransform(leftHand, rightHand).position, Quaternion.identity);
                projectileInstance.SetTarget(target, instigator,calculatedDamage);
        
           
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }
        private Transform GetWeaponTransform(Transform leftHand, Transform rightHand)
        {
            if (isRightHanded)
            {
                return rightHand;
            }
            else
            {
                return leftHand;
            }

        }
        public float GetDamage()
        {
            return weaponDamage;
        }
        public float GetRange()
        {
            return weaponRange;
        }
        public float GetPercentageBonus()
        {
            return percentageBonus;
        }
    }
}

