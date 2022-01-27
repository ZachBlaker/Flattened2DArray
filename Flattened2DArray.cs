#define validate
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

[Serializable]
public class Flattened2DArray<T> : IEnumerable
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

    //Constructor that copies existing Flattened2DArray that stores ValueType
    public Flattened2DArray(Flattened2DArray<T> original)
    {
        if (!typeof(T).IsValueType)
            throw new Exception($"Cannot copy Flattened2DArray storing {typeof(T)} as it is reference type");

        _width = original.width;
        _height = original.height;
        contents = new T[width * height];
        original.contents.CopyTo(contents,0);
    }


    //Set every position to same value
    public virtual void SetAll(T value)
    {
        for (int i = 0; i < contents.Length; i++)
            contents[i] = value;
    }


    //Used to directly modify the contents using index, if validation is required redirect to Set() and Get()
    public T this[int index]
    {
#if validate
        //Uses Get() / Set() which is slower but includes validation
        get => Get(FromIndex(index));
        set => Set(FromIndex(index), value);
#else
        get => contents[index];
        set => contents[index] = value;
#endif
    }


    //Convert x,y position into index value
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public int ToIndex(int x, int y)
    {
        return x * _width + y;
    }
    //Convert Vector2Int position into index value
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public int ToIndex(Vector2Int position)
    {
        return position.x * _width + position.y;
    }
    //Convert index into Vector2int position
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public Vector2Int FromIndex(int index)
    {
        return new Vector2Int(
            index / _width, 
            index % _width);
    }


    //Set the value of a position
    public virtual void Set(int x, int y, T value)
    {
#if validate
        try
        {
            contents[x * _width + y] = value;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message + $" At position {x},{y}");
        }
#else
        contents[x * _width + y] = value;
#endif
    }
    public virtual void Set(Vector2Int position, T value)
        => Set(position.x, position.y, value);


    //Get the value of a position
    public virtual T Get(int x, int y)
    {
#if validate
        try
        {
            return contents[x * _width + y];
        }
        catch(Exception e)
        {
            throw new Exception(e.Message + $" At position {x},{y}");
        }
#else
        return contents[x * _width + y];
#endif
    }
    public virtual T Get(Vector2Int position)
        => Get(position.x, position.y);


    //Get array of values from adjacent positions
    public virtual List<T> GetAdjacent(Vector2Int position, DirectionType directionType = DirectionType.Cardinal)
    {
        List<T> contents = new List<T>();
        Vector2Int[] adjacentPositions = Directions.GetAdjacentPositions(position, directionType);

        foreach (Vector2Int adjacentPosition in adjacentPositions)
            if (IsWithinBounds(adjacentPosition))
                contents.Add(Get(adjacentPosition));

        return contents;
    }
    public virtual List<T> GetAdjacent(int x, int y, DirectionType directionType = DirectionType.Cardinal)
        => GetAdjacent(new Vector2Int(x, y), directionType);
    public virtual List<T> GetAdjacent(int index, DirectionType directionType = DirectionType.Cardinal)
        => GetAdjacent(FromIndex(index), directionType);


    //Get the adjacent positions that are within bounds
    public virtual List<Vector2Int> GetValidAdjacentPositions(Vector2Int position, DirectionType directionType = DirectionType.Cardinal)
    {
        Vector2Int[] adjacentPositions = Directions.GetAdjacentPositions(position, directionType);

        List<Vector2Int> validPositions = new List<Vector2Int>();

        foreach (Vector2Int adjacentPosition in adjacentPositions)
            if (IsWithinBounds(adjacentPosition))
                validPositions.Add(adjacentPosition);

        return validPositions;
    }
    public virtual List<Vector2Int> GetValidAdjacentPositions(int x, int y, DirectionType directionType = DirectionType.Cardinal)
        => GetValidAdjacentPositions(new Vector2Int(x, y), directionType);
    public virtual List<Vector2Int> GetValidAdjacentPositions(int index, DirectionType directionType = DirectionType.Cardinal)
        => GetValidAdjacentPositions(FromIndex(index), directionType);


    //Get the direction of adjacent positions that are within bounds
    public virtual List<Vector2Int> GetValidDirections(Vector2Int position, DirectionType directionType = DirectionType.Cardinal)
    {
        Vector2Int[] directions = Directions.GetDirectionArray(directionType);
        List<Vector2Int> validDirections = new List<Vector2Int>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int tileInDirection = position + direction;
            if (IsWithinBounds(tileInDirection))
                validDirections.Add(direction);
        }
        return validDirections;
    }
    public virtual List<Vector2Int> GetValidDirections(int x, int y, DirectionType directionType = DirectionType.Cardinal)
        => GetValidDirections(new Vector2Int(x, y), directionType);
    public virtual List<Vector2Int> GetValidDirections(int index, DirectionType directionType = DirectionType.Cardinal)
        => GetValidDirections(FromIndex(index), directionType);


    //Checks if the position is within the bounds of the 2d array
    public bool IsWithinBounds(int x, int y)
    {
        if (x < 0 || x >= _width)    //Outside bounds on x axis
            return false;
        if (y < 0 || y >= _height)    //Outside bounds on y axis
            return false;
        return true;
    }
    public bool IsWithinBounds(Vector2Int position)
        => IsWithinBounds(position.x, position.y);
    public bool IsWithinBounds(int index)
        => IsWithinBounds(FromIndex(index));


    //Get a texture2D representing the contents of the 2d array
    public virtual Texture2D ToTexture2D(T positiveValue)
    {
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Get(x, y).Equals(positiveValue))
                    texture.SetPixel(x, y, Color.white);
                else
                    texture.SetPixel(x, y, Color.black);
            }
        }
        texture.Apply();
        return texture;
    }



    //Allows use of foreach(T item in Flattened2DArray)
#region foreach IEnumeration
    public IEnumerator GetEnumerator()
    {
        return new Flattened2DArrayEnumerator(this);
    }

    private class Flattened2DArrayEnumerator : IEnumerator
    {
        private Flattened2DArray<T> target;
        private int foreachPosition = -1;

        public Flattened2DArrayEnumerator(Flattened2DArray<T> target)
        {
            this.target = target;
        }

        //IEnumerator
        public bool MoveNext()
        {
            foreachPosition++;
            return (foreachPosition < target.contents.Length);
        }

        //IEnumerable
        public void Reset()
        {
            foreachPosition = -1;
        }

        //IEnumerable
        public object Current
        {
            get => target[foreachPosition];
            set => target[foreachPosition] = (T)value;
        }

    }
#endregion
}
