using System.Collections.Generic;

public interface IAbilityQueue
{
    void InitializeQueue();
    AbilityStruct DequeueAbility();
    void QueueAbility(AbilityStruct ability);
    bool CheckEmptyQueue();
}
