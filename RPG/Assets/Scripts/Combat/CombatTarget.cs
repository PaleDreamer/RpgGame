using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.resources;
using RPG.control;
namespace RPG.combat{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            
            
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) { return false; }
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);

            }
            return true;
        }
    }

}
