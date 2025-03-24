using UnityEngine;

public class SandSpider : EnemyClass
{
    private void Awake()
    {
        CreateEntity();
    }
    private void CreateEntity()
    {
        Health = 100;
        Damage = 10;
        Speed = 1;
        Abilities.Add(new AbilityStruct{ name = "Lunge", damage = 5, probability = 0.5f, cooldown = 2, chargeTime = 5});
        Abilities.Add(new AbilityStruct{ name = "Bite", damage = 10, probability = 0.5f, cooldown = 8, chargeTime = 10});
    }
}
