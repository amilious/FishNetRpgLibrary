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
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using NUnit.Framework;
using UnityEngine;

namespace Amilious.Core.EditModeTests {
    
    
    public class DirectionTest {
        
        [Test]
        public void DirectionFromVector3IntPoints() {
            var pointA = Vector3Int.zero;
            var east = Vector3Int.right;
            var west = Vector3Int.left;
            var north = Vector3Int.forward;
            var south = Vector3Int.back;
            var up = Vector3Int.up;
            var down = Vector3Int.down;
            var northWest = north + west;
            var northEast = north + east;
            var southWest = south + west;
            var southEast = south + east;
            var upWest = up + west;
            var upNorth = up + north;
            var upEast = up + east;
            var upSouth = up + south;
            var downWest = down + west;
            var downNorth = down + north;
            var downEast = down + east;
            var downSouth = down + south;
            var upNorthWest = northWest + up;
            var upNorthEast = northEast + up;
            var upSouthWest = southWest + up;
            var upSouthEast = southEast + up;
            var downNorthWest = northWest + down;
            var downNorthEast = northEast + down;
            var downSouthWest = southWest + down;
            var downSouthEast = southEast + down;
            Assert.AreEqual(Direction.East,pointA.DirectionToPoint(east));
            Assert.AreEqual(Direction.West,pointA.DirectionToPoint(west));
            Assert.AreEqual(Direction.North,pointA.DirectionToPoint(north));
            Assert.AreEqual(Direction.South,pointA.DirectionToPoint(south));
            Assert.AreEqual(Direction.Up,pointA.DirectionToPoint(up));
            Assert.AreEqual(Direction.Down,pointA.DirectionToPoint(down));
            Assert.AreEqual(Direction.NorthWest,pointA.DirectionToPoint(northWest));
            Assert.AreEqual(Direction.NorthEast,pointA.DirectionToPoint(northEast));
            Assert.AreEqual(Direction.SouthWest,pointA.DirectionToPoint(southWest));
            Assert.AreEqual(Direction.SouthEast,pointA.DirectionToPoint(southEast));
            Assert.AreEqual(Direction.UpWest,pointA.DirectionToPoint(upWest));
            Assert.AreEqual(Direction.UpNorth,pointA.DirectionToPoint(upNorth));
            Assert.AreEqual(Direction.UpEast,pointA.DirectionToPoint(upEast));
            Assert.AreEqual(Direction.UpSouth,pointA.DirectionToPoint(upSouth));
            Assert.AreEqual(Direction.DownWest,pointA.DirectionToPoint(downWest));
            Assert.AreEqual(Direction.DownEast,pointA.DirectionToPoint(downEast));
            Assert.AreEqual(Direction.DownNorth,pointA.DirectionToPoint(downNorth));
            Assert.AreEqual(Direction.DownSouth,pointA.DirectionToPoint(downSouth));
            Assert.AreEqual(Direction.UpNorthWest,pointA.DirectionToPoint(upNorthWest));
            Assert.AreEqual(Direction.UpNorthEast,pointA.DirectionToPoint(upNorthEast));
            Assert.AreEqual(Direction.UpSouthWest,pointA.DirectionToPoint(upSouthWest));
            Assert.AreEqual(Direction.UpSouthEast,pointA.DirectionToPoint(upSouthEast));
            Assert.AreEqual(Direction.DownNorthWest,pointA.DirectionToPoint(downNorthWest));
            Assert.AreEqual(Direction.DownNorthEast,pointA.DirectionToPoint(downNorthEast));
            Assert.AreEqual(Direction.DownSouthWest,pointA.DirectionToPoint(downSouthWest));
            Assert.AreEqual(Direction.DownSouthEast,pointA.DirectionToPoint(downSouthEast));
        }
        
        [Test]
        public void DirectionFromVector3Points() {
            var pointA = Vector3.zero;
            var east = Vector3.right;
            var west = Vector3.left;
            var north = Vector3.forward;
            var south = Vector3.back;
            var up = Vector3.up;
            var down = Vector3.down;
            var northWest = north + west;
            var northEast = north + east;
            var southWest = south + west;
            var southEast = south + east;
            var upWest = up + west;
            var upNorth = up + north;
            var upEast = up + east;
            var upSouth = up + south;
            var downWest = down + west;
            var downNorth = down + north;
            var downEast = down + east;
            var downSouth = down + south;
            var upNorthWest = northWest + up;
            var upNorthEast = northEast + up;
            var upSouthWest = southWest + up;
            var upSouthEast = southEast + up;
            var downNorthWest = northWest + down;
            var downNorthEast = northEast + down;
            var downSouthWest = southWest + down;
            var downSouthEast = southEast + down;
            Assert.AreEqual(Direction.East,pointA.DirectionToPoint(east));
            Assert.AreEqual(Direction.West,pointA.DirectionToPoint(west));
            Assert.AreEqual(Direction.North,pointA.DirectionToPoint(north));
            Assert.AreEqual(Direction.South,pointA.DirectionToPoint(south));
            Assert.AreEqual(Direction.Up,pointA.DirectionToPoint(up));
            Assert.AreEqual(Direction.Down,pointA.DirectionToPoint(down));
            Assert.AreEqual(Direction.NorthWest,pointA.DirectionToPoint(northWest));
            Assert.AreEqual(Direction.NorthEast,pointA.DirectionToPoint(northEast));
            Assert.AreEqual(Direction.SouthWest,pointA.DirectionToPoint(southWest));
            Assert.AreEqual(Direction.SouthEast,pointA.DirectionToPoint(southEast));
            Assert.AreEqual(Direction.UpWest,pointA.DirectionToPoint(upWest));
            Assert.AreEqual(Direction.UpNorth,pointA.DirectionToPoint(upNorth));
            Assert.AreEqual(Direction.UpEast,pointA.DirectionToPoint(upEast));
            Assert.AreEqual(Direction.UpSouth,pointA.DirectionToPoint(upSouth));
            Assert.AreEqual(Direction.DownWest,pointA.DirectionToPoint(downWest));
            Assert.AreEqual(Direction.DownEast,pointA.DirectionToPoint(downEast));
            Assert.AreEqual(Direction.DownNorth,pointA.DirectionToPoint(downNorth));
            Assert.AreEqual(Direction.DownSouth,pointA.DirectionToPoint(downSouth));
            Assert.AreEqual(Direction.UpNorthWest,pointA.DirectionToPoint(upNorthWest));
            Assert.AreEqual(Direction.UpNorthEast,pointA.DirectionToPoint(upNorthEast));
            Assert.AreEqual(Direction.UpSouthWest,pointA.DirectionToPoint(upSouthWest));
            Assert.AreEqual(Direction.UpSouthEast,pointA.DirectionToPoint(upSouthEast));
            Assert.AreEqual(Direction.DownNorthWest,pointA.DirectionToPoint(downNorthWest));
            Assert.AreEqual(Direction.DownNorthEast,pointA.DirectionToPoint(downNorthEast));
            Assert.AreEqual(Direction.DownSouthWest,pointA.DirectionToPoint(downSouthWest));
            Assert.AreEqual(Direction.DownSouthEast,pointA.DirectionToPoint(downSouthEast));
        }
        
        [Test]
        public void DirectionFromVector2IntXZPoints() {
            var pointA = Vector2Int.zero;
            var east = Vector2Int.right;
            var west = Vector2Int.left;
            var north = Vector2Int.up;
            var south = Vector2Int.down;
            var northWest = north + west;
            var northEast = north + east;
            var southWest = south + west;
            var southEast = south + east;
            Assert.AreEqual(Direction.East,pointA.DirectionToPointXZ(east));
            Assert.AreEqual(Direction.West,pointA.DirectionToPointXZ(west));
            Assert.AreEqual(Direction.North,pointA.DirectionToPointXZ(north));
            Assert.AreEqual(Direction.South,pointA.DirectionToPointXZ(south));
            Assert.AreEqual(Direction.NorthWest,pointA.DirectionToPointXZ(northWest));
            Assert.AreEqual(Direction.NorthEast,pointA.DirectionToPointXZ(northEast));
            Assert.AreEqual(Direction.SouthWest,pointA.DirectionToPointXZ(southWest));
            Assert.AreEqual(Direction.SouthEast,pointA.DirectionToPointXZ(southEast));
        }
        
        [Test]
        public void DirectionFromVector2XZPoints() {
            var pointA = Vector2.zero;
            var east = Vector2.right;
            var west = Vector2.left;
            var north = Vector2.up;
            var south = Vector2.down;
            var northWest = north + west;
            var northEast = north + east;
            var southWest = south + west;
            var southEast = south + east;
            Assert.AreEqual(Direction.East,pointA.DirectionToPointXZ(east));
            Assert.AreEqual(Direction.West,pointA.DirectionToPointXZ(west));
            Assert.AreEqual(Direction.North,pointA.DirectionToPointXZ(north));
            Assert.AreEqual(Direction.South,pointA.DirectionToPointXZ(south));
            Assert.AreEqual(Direction.NorthWest,pointA.DirectionToPointXZ(northWest));
            Assert.AreEqual(Direction.NorthEast,pointA.DirectionToPointXZ(northEast));
            Assert.AreEqual(Direction.SouthWest,pointA.DirectionToPointXZ(southWest));
            Assert.AreEqual(Direction.SouthEast,pointA.DirectionToPointXZ(southEast));
        }
        
        [Test]
        public void DirectionFromVector2IntXYPoints() {
            var pointA = Vector2Int.zero;
            var east = Vector2Int.right;
            var west = Vector2Int.left;
            var up = Vector2Int.up;
            var down = Vector2Int.down;
            var upWest = up + west;
            var upEast = up + east;
            var downWest = down + west;
            var downEast = down + east;
            Assert.AreEqual(Direction.East,pointA.DirectionToPointXY(east));
            Assert.AreEqual(Direction.West,pointA.DirectionToPointXY(west));
            Assert.AreEqual(Direction.Up,pointA.DirectionToPointXY(up));
            Assert.AreEqual(Direction.Down,pointA.DirectionToPointXY(down));
            Assert.AreEqual(Direction.UpWest,pointA.DirectionToPointXY(upWest));
            Assert.AreEqual(Direction.UpEast,pointA.DirectionToPointXY(upEast));
            Assert.AreEqual(Direction.DownWest,pointA.DirectionToPointXY(downWest));
            Assert.AreEqual(Direction.DownEast,pointA.DirectionToPointXY(downEast));
        }
        
        [Test]
        public void DirectionFromVector2XYPoints() {
            var pointA = Vector2.zero;
            var east = Vector2.right;
            var west = Vector2.left;
            var up = Vector2.up;
            var down = Vector2.down;
            var upWest = up + west;
            var upEast = up + east;
            var downWest = down + west;
            var downEast = down + east;
            Assert.AreEqual(Direction.East,pointA.DirectionToPointXY(east));
            Assert.AreEqual(Direction.West,pointA.DirectionToPointXY(west));
            Assert.AreEqual(Direction.Up,pointA.DirectionToPointXY(up));
            Assert.AreEqual(Direction.Down,pointA.DirectionToPointXY(down));
            Assert.AreEqual(Direction.UpWest,pointA.DirectionToPointXY(upWest));
            Assert.AreEqual(Direction.UpEast,pointA.DirectionToPointXY(upEast));
            Assert.AreEqual(Direction.DownWest,pointA.DirectionToPointXY(downWest));
            Assert.AreEqual(Direction.DownEast,pointA.DirectionToPointXY(downEast));
        }
        
    }
}