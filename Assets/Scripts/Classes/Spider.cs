using System.Collections.Generic;
using UnityEngine;

public class Spider : EnemyClass
{
    private void Awake()
    {
        CreateEntity();
    }

    private void CreateEntity()
    {
        Health = 50;
        Damage = 5;
        Speed = 1;
        if (Abilities == null)
        {
            Abilities = new List<AbilityStruct>();
        }
        Abilities.Add(new AbilityStruct{ name = "Lunge", damage = 5, probability = 0.5f, cooldown = 2, chargeTime = 5});
        Abilities.Add(new AbilityStruct{ name = "Bite", damage = 10, probability = 0.5f, cooldown = 8, chargeTime = 10});
    }
}
