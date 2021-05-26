using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;
namespace RPG.stats
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float experiencePoints = 0;
       // public delegate void ExperiencedGainedDelegate();
        public event Action onExperienceGained;
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }
        public float GetExperience()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            float exp = (float)state;
            experiencePoints = exp;
        }
    }
}

