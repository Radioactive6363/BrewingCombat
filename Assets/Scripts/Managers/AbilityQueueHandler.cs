public class AbilityQueueHandler : IAbilityQueue
{
    private AbilityStruct[] abilityQueue; 
    private int front; 
    private int back;
    private const int initialCapacity = 10; 

    public AbilityQueueHandler()
    {
        abilityQueue = new AbilityStruct[initialCapacity];
        front = 0; 
        back = 0;
    }

    // Initialize the queue
    public void InitializeQueue()
    {
        abilityQueue = new AbilityStruct[initialCapacity];
        front = 0;
        back = 0;
    }

    // Enqueue an ability (add it to the back of the queue)
    public void QueueAbility(AbilityStruct ability)
    {
        if ((back + 1) % abilityQueue.Length == front)  // Check if the queue is full
        {
            ResizeQueue();
        }

        abilityQueue[back] = ability;  // Add ability to the back
        back = (back + 1) % abilityQueue.Length;  // Move the back pointer forward
    }

    // Dequeue an ability (remove it from the front of the queue)
    public AbilityStruct DequeueAbility()
    {
        if (CheckEmptyQueue())
        {
            throw new System.InvalidOperationException("Queue is empty.");
        }

        AbilityStruct dequeuedAbility = abilityQueue[front];  // Get the ability at the front
        abilityQueue[front] = default;  // Clear the value at the front 
        front = (front + 1) % abilityQueue.Length;  // Move the front pointer forward

        return dequeuedAbility;
    }

    // Check if the queue is empty
    public bool CheckEmptyQueue()
    {
        return front == back;  // The queue is empty if front equals back
    }

    // Resize the queue when it's full
    private void ResizeQueue()
    {
        int newCapacity = abilityQueue.Length * 2;  // Double the capacity
        AbilityStruct[] newQueue = new AbilityStruct[newCapacity];

        int currentSize = (back - front + abilityQueue.Length) % abilityQueue.Length;
        for (int i = 0; i < currentSize; i++)
        {
            newQueue[i] = abilityQueue[(front + i) % abilityQueue.Length];  // Copy existing elements to the new queue
        }

        abilityQueue = newQueue;  // Replace the old queue with the new, larger queue
        front = 0;  // Reset front to 0
        back = currentSize;  // Set back to the current size
    }
}
