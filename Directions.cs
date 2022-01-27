using UnityEngine;
using System;

public enum DirectionType
{
    Cardinal,
    InterCardinal,
    All
}

public static class Directions
{   
    public static readonly Vector2Int Up = new Vector2Int(0, 1);
    public static readonly Vector2Int Right = new Vector2Int(1, 0);
    public static readonly Vector2Int Down = new Vector2Int(0, -1);
    public static readonly Vector2Int Left = new Vector2Int(-1, 0);

    public static readonly Vector2Int UpRight = new Vector2Int(1, 1);
    public static readonly Vector2Int DownRight = new Vector2Int(1, -1);
    public static readonly Vector2Int DownLeft = new Vector2Int(-1, -1);
    public static readonly Vector2Int UpLeft = new Vector2Int(-1, 1);

    public static readonly Vector2Int[] cardinalDirections = new Vector2Int[]
    {
        Up,
        Right,
        Down,
        Left
    };
    public static readonly Vector2Int[] interCardinalDirections = new Vector2Int[]
    {
        UpRight,
        DownRight,
        DownLeft,
        UpLeft
    };
    public static readonly Vector2Int[] allDirections = new Vector2Int[]
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    };


    private static readonly string[] directionNames = new string[]
    {
        "Up",
        "UpRight",
        "Right",
        "DownRight",
        "Down",
        "DownLeft",
        "Left",
        "UpLeft"
    };

    public static string GetName(this Vector2Int direction)
    {
        int index = Array.IndexOf(allDirections, direction);
        return directionNames[index];
    }

    public static Vector2Int[] GetAdjacentPositions(Vector2Int position, DirectionType directionType)
    {
        Vector2Int[] directions = directionType.GetDirectionArray();
        Vector2Int[] adjacentPositions = new Vector2Int[directions.Length];

        for (int i = 0; i < directions.Length; i++)
            adjacentPositions[i] = position + directions[i];
        return adjacentPositions;
    }

    public static Vector2Int[] GetDirectionArray(this DirectionType directionType)
    {
        switch (directionType)
        {
            case DirectionType.Cardinal:
                return cardinalDirections;
            case DirectionType.InterCardinal:
                return interCardinalDirections;
            case DirectionType.All:
                return allDirections;
            default:
                throw new Exception();
        }
    }
    public static bool IsValidDirection(Vector2Int direction)
    {
        foreach(Vector2Int dir in allDirections)
        {
            if (dir == direction)
                return true;
        }
        return false;
    }
}
