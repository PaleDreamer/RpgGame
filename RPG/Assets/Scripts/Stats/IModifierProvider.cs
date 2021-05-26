using System.Collections.Generic;
using UnityEngine;
namespace RPG.stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}

