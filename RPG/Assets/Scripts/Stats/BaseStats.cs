using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Utils;
namespace RPG.stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int StartLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;
        LazyValue<int>  currentLevel ;

        public event Action onLevelUp;
        Experience experience ;
        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start()
        {
            currentLevel.ForceInit();
            
           
        }
        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }
        public float GetStat(Stat stat)
        {
    
            return (GetBaseStat(stat) + GetAdditiveModifier(stat))*(1+GetPercentageModifier(stat)/100);
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float item in modifierProvider.GetPercentageModifiers(stat))
                {
                    total += item;
                }
            }
            return total;
        }

        private float GetBaseStat(Stat stat)
        {
    //        print(gameObject.name+ " currentLevel: " + currentLevel);
            return progression.GetStat(stat, characterClass, currentLevel.value);
        }
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach(IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach(float item in modifierProvider.GetAdditiveModifier(stat))
                {
                    total += item;
                }
            }
            return total;
        }
      
        public int GetLevel()
        {
         
            return currentLevel.value;
        }
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null)
            {
                return StartLevel;
            }

            float currentExp = experience.GetExperience();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                var expForLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (currentExp < expForLevelUp)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }

   
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;

                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpParticleEffect != null)
            {
                Instantiate(levelUpParticleEffect, transform);
            }
        }

       
    }
    
}
