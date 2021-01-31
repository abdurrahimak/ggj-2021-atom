using System;
using System.Collections.Generic;

public class Bag<T>
{
    private Queue<T> _itemQueue = new Queue<T>();
    private int _bagSize;

    public event Action<List<T>> QueueUpdated;
    public event Action<T> ItemRemoved;

    public Bag(int bagSize)
    {
        _bagSize = bagSize;
        _itemQueue = new Queue<T>(bagSize);
    }

    public void AddItemKey(T key)
    {
        if (_itemQueue.Count == _bagSize)
        {
            ItemRemoved?.Invoke(_itemQueue.Dequeue());
        }
        _itemQueue.Enqueue(key);
        QueueUpdated?.Invoke(new List<T>(_itemQueue));
    }
}
