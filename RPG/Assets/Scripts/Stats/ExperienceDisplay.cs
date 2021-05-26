using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.stats;
namespace RPG.resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {

            GetComponent<Text>().text = string.Format( "{0}",experience.GetExperience() );
           
        }
    }
}

