using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using RPG.resources;

namespace RPG.combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health;
        private void Awake()
        {
            GetComponent<Text>().text = string.Format("N/A");
        }

        private void Update()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
            if (health != null)
            {
                GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
            

        }
    }
}

