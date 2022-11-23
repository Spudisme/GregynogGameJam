using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue<T>
{
    // First in first out data method
    private List<T> queue = new List<T>();
    public T Pop() {
        T value = queue[0];
        queue.RemoveAt(0);
        return value;
    }

    public void Push(T value) {
        queue.Add(value);
    }

    public List<T> GetList() {
        return queue;
    }
}
