using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;
namespace RPG.sceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }
        [SerializeField] int levelIndex = 0;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime=3f;
        [SerializeField] float fadeInTime=1f;
        private void OnTriggerEnter(Collider other)
        {
           
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {

         
            
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(3f);
            //save current level
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(levelIndex);
            print("loading completed");

            
            Portal otherPortal = GetOtherPortal();
            wrapper.Load();
            UpdatePlayer(otherPortal);
            //    print("Portal is " + otherPortal.destination);
            wrapper.Save();
            yield return new WaitForSeconds(1f);
            yield return fader.FadeIn(1f);
            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            print("Portal is "+otherPortal.destination);
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) { continue; }
                if(portal.destination==this.destination)
                   return portal;

            }
            return null;
        }
    }
}

