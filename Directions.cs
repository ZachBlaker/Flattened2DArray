using UnityEngine;
using System;

public enum DirectionType
{
    Cardinal,
    InterCardinal,
    All
}
public enum DirectionEnum
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
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


    //Returns an array of Vector2Int positions that are adjacent to position in the given DirectionType
    public static Vector2Int[] GetAdjacentPositions(Vector2Int position, DirectionType directionType)
    {
        Vector2Int[] directions = directionType.GetDirectionArray();
        Vector2Int[] adjacentPositions = new Vector2Int[directions.Length];

        for (int i = 0; i < directions.Length; i++)
            adjacentPositions[i] = position + directions[i];
        return adjacentPositions;
    }

    //Returns the Vector2Int array for that DirectionType
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

    public static Vector2Int ToVector2Int(this DirectionEnum directionEnum)
    {
        return allDirections[(int)directionEnum];
    }
}
