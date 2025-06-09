using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface SetTDA
{
    void InitializeSet();
    bool IsSetEmpty();
    void Add(int x);
    int Choose();
    void Remove(int x);
    bool Contains(int x);
}

public class Node
{
    public int data;
    public Node next;
}

// DYNAMIC IMPLEMENTATION //
public class LinkedListSet : SetTDA
{
    Node head;

    public void InitializeSet()
    {
        head = null;
    }

    public bool IsSetEmpty()
    {
        return (head == null);
    }

    public void Add(int x)
    {
        // Check that x is not already in the set
        if (!this.Contains(x))
        {
            Node temp = new Node();
            temp.data = x;
            temp.next = head;
            head = temp;
        }
    }

    public int Choose()
    {
        return head.data;
    }

    public void Remove(int x)
    {
        if (head != null)
        {
            // If it's the first element in the list
            if (head.data == x)
            {
                head = head.next;
            }
            else
            {
                Node temp = head;
                while (temp.next != null && temp.next.data != x)
                    temp = temp.next;
                if (temp.next != null)
                    temp.next = temp.next.next;
            }
        }
    }

    public bool Contains(int x)
    {
        Node temp = head;
        while (temp != null && temp.data != x)
        {
            temp = temp.next;
        }
        return (temp != null);
    }
}

// STATIC IMPLEMENTATION //
public class ArraySet : SetTDA
{
    int[] array;
    int count;

    public void Add(int x)
    {
        if (!this.Contains(x))
        {
            array[count] = x;
            count++;
        }
    }

    public bool IsSetEmpty()
    {
        return count == 0;
    }

    public int Choose()
    {
        return array[count - 1];
    }

    public void InitializeSet()
    {
        array = new int[100];
        count = 0;
    }

    public bool Contains(int x)
    {
        int i = 0;
        while (i < count && array[i] != x)
        {
            i++;
        }
        return (i < count);
    }

    public void Remove(int x)
    {
        int i = 0;
        while (i < count && array[i] != x)
        {
            i++;
        }
        if (i < count)
        {
            array[i] = array[count - 1];
            count--;
        }
    }
}
