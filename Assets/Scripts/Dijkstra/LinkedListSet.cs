using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISetADT
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
    public int Data;
    public Node Next;
}

// DYNAMIC IMPLEMENTATION //
public class LinkedListSet : ISetADT
{
    Node _head;

    public void InitializeSet()
    {
        _head = null;
    }

    public bool IsSetEmpty()
    {
        return (_head == null);
    }

    public void Add(int x)
    {
        // Check that x is not already in the set
        if (!this.Contains(x))
        {
            Node temp = new Node();
            temp.Data = x;
            temp.Next = _head;
            _head = temp;
        }
    }

    public int Choose()
    {
        return _head.Data;
    }

    public void Remove(int x)
    {
        if (_head != null)
        {
            // If it's the first element in the list
            if (_head.Data == x)
            {
                _head = _head.Next;
            }
            else
            {
                Node temp = _head;
                while (temp.Next != null && temp.Next.Data != x)
                    temp = temp.Next;
                if (temp.Next != null)
                    temp.Next = temp.Next.Next;
            }
        }
    }

    public bool Contains(int x)
    {
        Node temp = _head;
        while (temp != null && temp.Data != x)
        {
            temp = temp.Next;
        }
        return (temp != null);
    }
}

// STATIC IMPLEMENTATION //
public class ArraySet : ISetADT
{
    int[] _array;
    int _count;

    public void Add(int x)
    {
        if (!this.Contains(x))
        {
            _array[_count] = x;
            _count++;
        }
    }

    public bool IsSetEmpty()
    {
        return _count == 0;
    }

    public int Choose()
    {
        return _array[_count - 1];
    }

    public void InitializeSet()
    {
        _array = new int[100];
        _count = 0;
    }

    public bool Contains(int x)
    {
        int i = 0;
        while (i < _count && _array[i] != x)
        {
            i++;
        }
        return (i < _count);
    }

    public void Remove(int x)
    {
        int i = 0;
        while (i < _count && _array[i] != x)
        {
            i++;
        }
        if (i < _count)
        {
            _array[i] = _array[_count - 1];
            _count--;
        }
    }
}
