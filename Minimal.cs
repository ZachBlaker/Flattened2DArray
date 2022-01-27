using UnityEngine;
using System;

[Serializable]
public class Flattened2DArray<T>
{
    public int width { get { return _width; } }
    public int height { get { return _height; } }

    public int size => width * height;

    [SerializeField] protected int _width;
    [SerializeField] protected int _height;

    [SerializeField] protected T[] contents;

    //Constructor
    public Flattened2DArray(int width, int height)
    {
        _width = width;
        _height = height;
        contents = new T[width * height];
    }

    //Set every position to same value
    public virtual void SetAll(T value)
    {
        for (int i = 0; i < contents.Length; i++)
            contents[i] = value;
    }

    //Set the value of a position
    public virtual void Set(int x, int y, T value)
    {
        contents[x * _width + y] = value;
    }
    public virtual void Set(Vector2Int position, T value)
        => Set(position.x, position.y, value);


    //Get the value of a position
    public virtual T Get(int x, int y)
    {
        return contents[x * _width + y];
    }
    public virtual T Get(Vector2Int position)
        => Get(position.x, position.y);
}
