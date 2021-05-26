using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        static bool hasspawned = false;
        private void Awake()
        {
            if (hasspawned) return;
            SpawnPersistentObjects();
            hasspawned = true;
        }
        private void SpawnPersistentObjects()
        {
            GameObject gameObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(gameObject);
        }
    }
}

