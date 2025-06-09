public class AbilityQueueHandler : IAbilityQueue
{
    private AbilityStruct[] _abilityQueue; 
    private int _front; 
    private int _back;
    private const int InitialCapacity = 10; 

    public AbilityQueueHandler()
    {
        _abilityQueue = new AbilityStruct[InitialCapacity];
        _front = 0; 
        _back = 0;
    }

    // Initialize the queue
    public void InitializeQueue()
    {
        _abilityQueue = new AbilityStruct[InitialCapacity];
        _front = 0;
        _back = 0;
    }

    // Enqueue an ability (add it to the back of the queue)
    public void QueueAbility(AbilityStruct ability)
    {
        if ((_back + 1) % _abilityQueue.Length == _front)  // Check if the queue is full
        {
            ResizeQueue();
        }

        _abilityQueue[_back] = ability;  // Add ability to the back
        _back = (_back + 1) % _abilityQueue.Length;  // Move the back pointer forward
    }

    // Dequeue an ability (remove it from the front of the queue)
    public AbilityStruct DequeueAbility()
    {
        if (CheckEmptyQueue())
        {
            throw new System.InvalidOperationException("Queue is empty.");
        }

        AbilityStruct dequeuedAbility = _abilityQueue[_front];  // Get the ability at the front
        _abilityQueue[_front] = default;  // Clear the value at the front 
        _front = (_front + 1) % _abilityQueue.Length;  // Move the front pointer forward

        return dequeuedAbility;
    }

    // Check if the queue is empty
    public bool CheckEmptyQueue()
    {
        return _front == _back;  // The queue is empty if front equals back
    }

    // Resize the queue when it's full
    private void ResizeQueue()
    {
        int newCapacity = _abilityQueue.Length * 2;  // Double the capacity
        AbilityStruct[] newQueue = new AbilityStruct[newCapacity];

        int currentSize = (_back - _front + _abilityQueue.Length) % _abilityQueue.Length;
        for (int i = 0; i < currentSize; i++)
        {
            newQueue[i] = _abilityQueue[(_front + i) % _abilityQueue.Length];  // Copy existing elements to the new queue
        }

        _abilityQueue = newQueue;  // Replace the old queue with the new, larger queue
        _front = 0;  // Reset front to 0
        _back = currentSize;  // Set back to the current size
    }
}
