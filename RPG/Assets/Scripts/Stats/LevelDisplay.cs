using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.stats;
namespace RPG.resources
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {

            GetComponent<Text>().text = string.Format( "{0}", baseStats.GetLevel());
           
        }
    }
}

