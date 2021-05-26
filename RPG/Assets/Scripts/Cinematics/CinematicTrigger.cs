using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.cinematic {
    
    public class CinematicTrigger : MonoBehaviour
    {
        private bool EnterDoor = false;
        private void OnTriggerEnter(Collider other)
        {
            if(!EnterDoor&& other.tag == "Player")
            {
       //         Debug.Log("Enter door");
                GetComponent<PlayableDirector>().Play();
                EnterDoor = true;
            }
      
        }

    }
}



