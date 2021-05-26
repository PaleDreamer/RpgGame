using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.control;
namespace RPG.combat
{
    public class WeaponPickup : MonoBehaviour,IRaycastable
    {
        [SerializeField] Weapon equippedWeapon;
        [SerializeField] float respawnTime = 5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }
        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(equippedWeapon);
            StartCoroutine(HideForSeconds());
        }
        private IEnumerator HideForSeconds()
        {
            
            ShowObject(false);
          
            yield return new WaitForSeconds(respawnTime);
            ShowObject(true);
            gameObject.SetActive(true);
        }

        private void ShowObject(bool shouldShow)
        {

            GetComponent<Collider>().enabled = shouldShow;
        
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
          
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}

