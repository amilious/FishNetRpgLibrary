/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using UnityEngine;
using System.Collections.Generic;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace Amilious.Core {
    
    /// <summary>
    /// This enum is used to represent 3d directions.
    /// </summary>
    [Serializable]
    public enum Direction {
        
        None = 0,
        
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        Up = 16,
        Down = 32,
        
        NorthWest = North | West,
        NorthEast = North | East,
        SouthWest = South | West,
        SouthEast = South | East,
        UpNorth = Up | North,
        UpEast = Up | East,
        UpSouth = Up | South,
        UpWest = Up | West ,
        DownNorth = Down | North,
        DownEast = Down | East,
        DownSouth = Down | South,
        DownWest = Down | West,
        
        UpNorthWest = Up | North | West,
        UpNorthEast = Up | North | East,
        UpSouthWest = Up | South | West,
        UpSouthEast = Up | South | East,
        DownNorthWest = Down | North | West,
        DownNorthEast = Down | North | East,
        DownSouthWest = Down | South | West,
        DownSouthEast = Down | South | East

    }
    
    
    /// <summary>
    /// This enum is use to represent the directions types.
    /// </summary>
    [Serializable]
    public enum DirectionType {
        AllDirections,
        SingleDirection,
        DoubleDirection,
        TripleDirection,
        XzSingleDirection,
        XzDoubleDirection,
        XySingleDirection,
        XyDoubleDirection,
        ZySingleDirection,
        ZyDoubleDirection
    }

    
    /// <summary>
    /// This static class is used to add extension methods to the Direction and Direction type enums.
    /// </summary>
    public static class DirectionUtils {

        #region Private Variables //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary is used to cache the Vector3Int representations of the directions.
        /// </summary>
        private static readonly Dictionary<Direction, Vector3Int> Vector3Ints = new ();
        
        /// <summary>
        /// This dictionary is used to cache the Vector2Int xz representations of the directions. 
        /// </summary>
        private static readonly Dictionary<Direction, Vector2Int> Vector2XZInts = new ();
        
        /// <summary>
        /// This dictionary is used to cache the Vector2Int xy representations of the directions. 
        /// </summary>
        private static readonly Dictionary<Direction, Vector2Int> Vector2XYInts = new ();
        
        #endregion
        
        
        #region Direction Type Methods /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the available directions for the direction type.
        /// </summary>
        /// <param name="type">The direction type.</param>
        /// <returns>The valid directions for the given direction type.</returns>
        public static IEnumerable<Direction> GetDirections(this DirectionType type) {
            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
                if(direction.IsDirectionType(type)) yield return direction;
        }

        /// <summary>
        /// This method is used to check if the direction is of the given direction type.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The direction type that you want to check for.</param>
        /// <returns>True if the direction is of the given type, otherwise false.</returns>
        public static bool IsDirectionType(this Direction direction, DirectionType type) {
            return type switch {
                DirectionType.AllDirections => direction!=Direction.None,
                DirectionType.SingleDirection => direction.IsSingleDirection(),
                DirectionType.DoubleDirection => direction.IsDoubleDirection(),
                DirectionType.TripleDirection => direction.IsTripleDirection(),
                DirectionType.XzSingleDirection => direction.IsXzSingleDirection(),
                DirectionType.XzDoubleDirection => direction.IsXzDoubleDirection(),
                DirectionType.XySingleDirection => direction.IsXySingleDirection(),
                DirectionType.XyDoubleDirection => direction.IsXyDoubleDirection(),
                DirectionType.ZySingleDirection => direction.IsZySingleDirection(),
                DirectionType.ZyDoubleDirection => direction.IsZyDoubleDirection(),
                _ => false
            };
        }

        /// <summary>
        /// This method is used to check if the direction is a triple direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a triple direction, otherwise false.</returns>
        public static bool IsTripleDirection(this Direction direction) => direction is Direction.UpNorthWest or
            Direction.UpNorthEast or Direction.UpSouthWest or Direction.UpSouthEast or Direction.DownNorthWest or
            Direction.DownNorthEast or Direction.DownSouthWest or Direction.DownSouthEast;

        /// <summary>
        /// This method is used to check if the direction is a double direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a double direction, otherwise false.</returns>
        public static bool IsDoubleDirection(this Direction direction) => direction is Direction.DownEast or
            Direction.DownNorth or Direction.DownSouth or Direction.DownWest or Direction.NorthEast or
            Direction.NorthWest or Direction.SouthEast or Direction.SouthWest or Direction.UpNorth or Direction.UpEast
            or Direction.UpSouth or Direction.UpWest;

        /// <summary>
        /// This method is used to check if the direction is a double direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a double direction, otherwise false.</returns>
        public static bool IsZyDoubleDirection(this Direction direction) => direction is Direction.UpNorth or
            Direction.UpSouth or Direction.DownNorth or Direction.DownSouth;

        /// <summary>
        /// This method is used to check if the direction is a double direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a double direction, otherwise false.</returns>
        public static bool IsXzDoubleDirection(this Direction direction) => direction is Direction.NorthEast or
            Direction.NorthWest or Direction.SouthEast or Direction.SouthWest;

        /// <summary>
        /// This method is used to check if the direction is a double direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a double direction, otherwise false.</returns>
        public static bool IsXyDoubleDirection(this Direction direction) => direction is Direction.UpWest or
            Direction.UpEast or Direction.DownWest or Direction.DownEast;
        
        /// <summary>
        /// This method is used to check if the direction is a single direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a single direction, otherwise false.</returns>
        public static bool IsSingleDirection(this Direction direction) => direction is Direction.Down or
            Direction.East or Direction.North or Direction.South or Direction.Up or Direction.West;
        
        /// <summary>
        /// This method is used to check if the direction is a single direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a single direction, otherwise false.</returns>
        public static bool IsZySingleDirection(this Direction direction) => direction is Direction.North or
            Direction.South or Direction.Up or Direction.Down;
        
        /// <summary>
        /// This method is used to check if the direction is a single direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a single direction, otherwise false.</returns>
        public static bool IsXzSingleDirection(this Direction direction) => direction is Direction.West or
            Direction.East or Direction.North or Direction.South;
        
        /// <summary>
        /// This method is used to check if the direction is a single direction.
        /// </summary>
        /// <param name="direction">The direction you want to check.</param>
        /// <returns>True if the direction is a single direction, otherwise false.</returns>
        public static bool IsXySingleDirection(this Direction direction) => direction is Direction.West or
            Direction.East or Direction.Up or Direction.Down;

        #endregion
        
        
        #region Get Vector Value From Direction ////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get a <see cref="Vector3Int"/> representation of the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The vector representation of the direction.</returns>
        public static Vector3Int GetVector3Int(this Direction direction) {
            //check cache
            if(Vector3Ints.TryGetValue(direction, out var value)) return value;
            //build value
            var x = direction.HasFlag(Direction.West) ? -1 : direction.HasFlag(Direction.East) ? 1 : 0;
            var y = direction.HasFlag(Direction.Down) ? -1 : direction.HasFlag(Direction.Up) ? 1 : 0;
            var z = direction.HasFlag(Direction.South) ? -1 : direction.HasFlag(Direction.North) ? 1 : 0;
            Vector3Ints[direction] = new Vector3Int(x, y, z);
            return Vector3Ints[direction];
        }
        
        /// <summary>
        /// This method is used to get a <see cref="Vector2Int"/> xz representation of the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The vector representation of the direction.</returns>
        public static Vector2Int GetVector2IntXZ(this Direction direction) {
            //check cache
            if(Vector2XZInts.TryGetValue(direction, out var value)) return value;
            //build value
            var x = direction.HasFlag(Direction.West) ? -1 : direction.HasFlag(Direction.East) ? 1 : 0;
            var z = direction.HasFlag(Direction.South) ? -1 : direction.HasFlag(Direction.North) ? 1 : 0;
            Vector2XZInts[direction] = new Vector2Int(x, z);
            return Vector2XZInts[direction];
        }
        
        /// <summary>
        /// This method is used to get a <see cref="Vector2Int"/> xy representation of the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The vector representation of the direction.</returns>
        public static Vector2Int GetVector2IntXY(this Direction direction) {
            //check cache
            if(Vector2XYInts.TryGetValue(direction, out var value)) return value;
            //build value
            var x = direction.HasFlag(Direction.West) ? -1 : direction.HasFlag(Direction.East) ? 1 : 0;
            var y = direction.HasFlag(Direction.Down) ? -1 : direction.HasFlag(Direction.Up) ? 1 : 0;
            Vector2XYInts[direction] = new Vector2Int(x, y);
            return Vector2XYInts[direction];
        }
        
        /// <summary>
        /// This method is used to get a <see cref="Vector3"/> representation of the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The vector representation of the direction.</returns>
        public static Vector3 GetVector3(this Direction direction) {
            return GetVector3Int(direction);
        }
        
        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> xz representation of the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The vector representation of the direction.</returns>
        public static Vector2 GetVector2XZ(this Direction direction) {
            return GetVector2IntXZ(direction);
        }
        
        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> xy representation of the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The vector representation of the direction.</returns>
        public static Vector2 GetVector2XY(this Direction direction) {
            return GetVector2IntXY(direction);
        }

        #endregion
        
        
        #region Get Direction From Points //////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the direction to the provided point.
        /// </summary>
        /// <param name="from">The base point to use for the direction.</param>
        /// <param name="to">The position that you want to get the direction of.</param>
        /// <returns>The direction to the point.</returns>
        public static Direction DirectionToPoint(this Vector3Int from, Vector3Int to) {
            var difX = to.x - from.x;
            var difY = to.y - from.y;
            var difZ = to.z - from.z;
            var direction = Direction.None;
            if(difX != 0) direction |= difX < 0 ? Direction.West : Direction.East;
            if(difZ != 0) direction |= difZ < 0 ? Direction.South : Direction.North;
            if(difY != 0) direction |= difY < 0 ? Direction.Down : Direction.Up;
            return direction;
        }
        
        /// <summary>
        /// This method is used to get the direction to the provided point.
        /// </summary>
        /// <param name="from">The base point to use for the direction.</param>
        /// <param name="to">The position that you want to get the direction of.</param>
        /// <returns>The direction to the point.</returns>
        public static Direction DirectionToPoint(this Vector3 from, Vector3 to) {
            var difX = to.x - from.x;
            var difY = to.y - from.y;
            var difZ = to.z - from.z;
            var direction = Direction.None;
            if(difX != 0) direction |= difX < 0 ? Direction.West : Direction.East;
            if(difZ != 0) direction |= difZ < 0 ? Direction.South : Direction.North;
            if(difY != 0) direction |= difY < 0 ? Direction.Down : Direction.Up;
            return direction;
        }

        /// <summary>
        /// This method is used to get the xz direction to the provided point.
        /// </summary>
        /// <param name="from">The base point to use for the direction.</param>
        /// <param name="to">The position that you want to get the direction of.</param>
        /// <returns>The direction to the point.</returns>
        public static Direction DirectionToPointXZ(this Vector2Int from, Vector2Int to) {
            var difX = to.x - from.x;
            var difZ = to.y - from.y;
            var direction = Direction.None;
            if(difX != 0) direction |= difX < 0 ? Direction.West : Direction.East;
            if(difZ != 0) direction |= difZ < 0 ? Direction.South : Direction.North;
            return direction;
        }
        
        /// <summary>
        /// This method is used to get the xy direction to the provided point.
        /// </summary>
        /// <param name="from">The base point to use for the direction.</param>
        /// <param name="to">The position that you want to get the direction of.</param>
        /// <returns>The direction to the point.</returns>
        public static Direction DirectionToPointXY(this Vector2Int from, Vector2Int to) {
            var difX = to.x - from.x;
            var difY = to.y - from.y;
            var direction = Direction.None;
            if(difX != 0) direction |= difX < 0 ? Direction.West : Direction.East;
            if(difY != 0) direction |= difY < 0 ? Direction.Down : Direction.Up;
            return direction;
        }
        
        /// <summary>
        /// This method is used to get the xz direction to the provided point.
        /// </summary>
        /// <param name="from">The base point to use for the direction.</param>
        /// <param name="to">The position that you want to get the direction of.</param>
        /// <returns>The direction to the point.</returns>
        public static Direction DirectionToPointXZ(this Vector2 from, Vector2 to) {
            var difX = to.x - from.x;
            var difZ = to.y - from.y;
            var direction = Direction.None;
            if(difX != 0) direction |= difX < 0 ? Direction.West : Direction.East;
            if(difZ != 0) direction |= difZ < 0 ? Direction.South : Direction.North;
            return direction;
        }
        
        /// <summary>
        /// This method is used to get the xy direction to the provided point.
        /// </summary>
        /// <param name="from">The base point to use for the direction.</param>
        /// <param name="to">The position that you want to get the direction of.</param>
        /// <returns>The direction to the point.</returns>
        public static Direction DirectionToPointXY(this Vector2 from, Vector2 to) {
            var difX = to.x - from.x;
            var difY = to.y - from.y;
            var direction = Direction.None;
            if(difX != 0) direction |= difX < 0 ? Direction.West : Direction.East;
            if(difY != 0) direction |= difY < 0 ? Direction.Down : Direction.Up;
            return direction;
        }
        
        #endregion
        
        
        #region Get Point In Direction /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the point in the given direction.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="direction">The direction of the point.</param>
        /// <param name="distance">The distance from the starting point.</param>
        /// <returns>The point in the given direction and distance from the starting point.</returns>
        public static Vector3Int GetPointInDirection(this Vector3Int start, Direction direction, int distance = 1) {
            return start + direction.GetVector3Int() * distance;
        }
        
        /// <summary>
        /// This method is used to get the point in the given direction.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="direction">The direction of the point.</param>
        /// <param name="distance">The distance from the starting point.</param>
        /// <returns>The point in the given direction and distance from the starting point.</returns>
        public static Vector3 GetPointInDirection(this Vector3 start, Direction direction, float distance = 1) {
            return start + direction.GetVector3() * distance;
        }
        
        /// <summary>
        /// This method is used to get the xz point in the given direction.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="direction">The direction of the point.</param>
        /// <param name="distance">The distance from the starting point.</param>
        /// <returns>The point in the given direction and distance from the starting point.</returns>
        public static Vector2Int GetPointInDirectionXZ(this Vector2Int start, Direction direction, int distance = 1) {
            return start + direction.GetVector2IntXZ() * distance;
        }
        
        /// <summary>
        /// This method is used to get the xy point in the given direction.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="direction">The direction of the point.</param>
        /// <param name="distance">The distance from the starting point.</param>
        /// <returns>The point in the given direction and distance from the starting point.</returns>
        public static Vector2Int GetPointInDirectionXY(this Vector2Int start, Direction direction, int distance = 1) {
            return start + direction.GetVector2IntXY() * distance;
        }
        
        /// <summary>
        /// This method is used to get the xz point in the given direction.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="direction">The direction of the point.</param>
        /// <param name="distance">The distance from the starting point.</param>
        /// <returns>The point in the given direction and distance from the starting point.</returns>
        public static Vector2 GetPointInDirectionXZ(this Vector2 start, Direction direction, float distance = 1) {
            return start + direction.GetVector2XZ() * distance;
        }
        
        /// <summary>
        /// This method is used to get the xy point in the given direction.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="direction">The direction of the point.</param>
        /// <param name="distance">The distance from the starting point.</param>
        /// <returns>The point in the given direction and distance from the starting point.</returns>
        public static Vector2 GetPointInDirectionXY(this Vector2 start, Direction direction, float distance = 1) {
            return start + direction.GetVector2XY() * distance;
        }
        
        #endregion
        
    }
    
}