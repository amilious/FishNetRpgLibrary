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
using System.Collections;
using System.Collections.Generic;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Amilious.Core {
    
    /// <summary>
    /// This class is used to cast for specific <see cref="MonoBehaviour"/>s or <see cref="GameObject"/>s.
    /// </summary>
    public class BufferedCaster {
        
        #region Fields /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold hits.
        /// </summary>
        public readonly RaycastHit[] RaycastBuffer;
        
        /// <summary>
        /// This field is used for sorting by distance.
        /// </summary>
        private readonly float[] _distanceBuffer;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the current number of items in the <see cref="RaycastBuffer"/>.
        /// </summary>
        public int HitSize { get; protected set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to create a new <see cref="BufferedCaster"/>.
        /// </summary>
        /// <param name="bufferSize">The max number of hits that
        /// can be returned from a cast.</param>
        public BufferedCaster(int bufferSize) {
            RaycastBuffer = new RaycastHit[bufferSize];
            _distanceBuffer = new float[bufferSize];
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Raycast ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to preform a ray cast using the allocated buffers.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the raycast.</returns>
        public IEnumerable<RaycastHit> Raycast(Ray ray, bool sortByDistance = false, 
            float maxDistance = float.PositiveInfinity , int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            DoRaycast(ray,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }
        
        /// <summary>
        /// This method is used to preform a ray cast using the allocated buffers.
        /// </summary>
        /// <param name="origin">The starting point and direction of the ray.</param>
        /// <param name="direction">The direction of the ray.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the raycast.</returns>
        public IEnumerable<RaycastHit> Raycast(Vector3 origin, Vector3 direction, bool sortByDistance = false, 
            float maxDistance = float.PositiveInfinity , int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            DoRaycast(origin,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }
        
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the raycast.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the raycast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the raycast. </returns>
        public IEnumerable<T> FilteredRaycast<T>(Ray ray, bool sortByDistance = false,
            float maxDistance = float.PositiveInfinity , int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T: MonoBehaviour {
            //do raycast
            DoRaycast(ray,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++)
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }
        
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the raycast.
        /// </summary>
        /// <param name="origin">The starting point and direction of the ray.</param>
        /// <param name="direction">The direction of the ray.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the raycast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the raycast. </returns>
        public IEnumerable<T> FilteredRaycast<T>(Vector3 origin, Vector3 direction, bool sortByDistance = false,
            float maxDistance = float.PositiveInfinity , int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T: MonoBehaviour {
            //do raycast
            DoRaycast(origin,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++)
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }
        
        
        /// <summary>
        /// Cast a ray through the Scene and store the hits into the buffer.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoRaycast(Ray ray, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.RaycastNonAlloc(ray, RaycastBuffer, maxDistance, layerMask, queryTriggerInteraction);
        }
        
        /// <summary>
        /// Cast a ray through the Scene and store the hits in the buffer.
        /// </summary>
        /// <param name="origin">The starting point and direction of the ray.</param>
        /// <param name="direction">The direction of the ray.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoRaycast(Vector3 origin, Vector3 direction, float maxDistance = float.PositiveInfinity, 
            int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.RaycastNonAlloc(origin, direction, RaycastBuffer, maxDistance, layerMask, queryTriggerInteraction);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region SphereCast /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to preform a sphere cast using the allocated buffers.
        /// </summary>
        /// <param name="origin">The center of the sphere at the start of the sweep.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction in which to sweep the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the sphere cast.</returns>
        public IEnumerable<RaycastHit> SphereCast(Vector3 origin, float radius, Vector3 direction,
            bool sortByDistance = false, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            DoSphereCast(origin,radius,direction,maxDistance,layerMask, queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }

        /// <summary>
        /// This method is used to preform a sphere cast using the allocated buffers.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the sphere cast.</returns>
        public IEnumerable<RaycastHit> SphereCast(Ray ray, float radius,
            bool sortByDistance = false, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            DoSphereCast(ray,radius,maxDistance,layerMask, queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }
       
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the sphere cast.
        /// </summary>
        /// <param name="origin">The center of the sphere at the start of the sweep.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction in which to sweep the sphere.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the sphere cast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the sphere cast. </returns>
        public IEnumerable<T> FilteredSphereCast<T>(Vector3 origin, float radius, Vector3 direction,
            bool sortByDistance = false, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            DoSphereCast(origin,radius,direction,maxDistance,layerMask, queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) 
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }
        
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the sphere cast.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the sphere cast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the sphere cast. </returns>
        public IEnumerable<T> FilteredSphereCast<T>(Ray ray, float radius, bool sortByDistance = false, 
            float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T: MonoBehaviour {
            //do the sphere cast
            DoSphereCast(ray,radius,maxDistance,layerMask, queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) 
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }

        /// <summary>
        /// Cast sphere along the direction and store the results in the buffer.
        /// </summary>
        /// <param name="origin">The center of the sphere at the start of the sweep.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction in which to sweep the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoSphereCast(Vector3 origin, in float radius, Vector3 direction,
            float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.SphereCastNonAlloc(origin, radius, direction, RaycastBuffer, maxDistance, layerMask, 
                queryTriggerInteraction);
        }
        
        /// <summary>
        /// Cast sphere along the direction and store hits in the buffer.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoSphereCast(Ray ray, in float radius, float maxDistance = float.PositiveInfinity, 
            int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.SphereCastNonAlloc(ray, radius, RaycastBuffer, maxDistance, layerMask,queryTriggerInteraction);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region BoxCast ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to preform a box cast using the allocated buffers.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the box cast.</returns>
        public IEnumerable<RaycastHit> BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, 
            bool sortByDistance, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            DoBoxCast(center,halfExtents,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }
        
        /// <summary>
        /// This method is used to preform a box cast using the allocated buffers.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">Rotation of the box.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the box cast.</returns>
        public IEnumerable<RaycastHit> BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, 
            Quaternion orientation, bool sortByDistance, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            DoBoxCast(center,halfExtents,direction,orientation,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }
        
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the box cast.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the box cast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the box cast. </returns>
        public IEnumerable<T> FilteredBoxCast<T>(Vector3 center, Vector3 halfExtents, Vector3 direction, 
            bool sortByDistance, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            DoBoxCast(center,halfExtents,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) 
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }
        
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the box cast.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">Rotation of the box.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the box cast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the box cast. </returns>
        public IEnumerable<T> FilteredBoxCast<T>(Vector3 center, Vector3 halfExtents, Vector3 direction, 
            Quaternion orientation, bool sortByDistance, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            DoBoxCast(center,halfExtents,direction, orientation, maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) 
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }

        /// <summary>
        /// Cast the box along the direction and store hits in the buffer.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction,
            float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.BoxCastNonAlloc(center, halfExtents, direction, RaycastBuffer, Quaternion.identity, maxDistance,
                layerMask, queryTriggerInteraction);
        }
        
        /// <summary>
        /// Cast the box along the direction and store hits in the buffer.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">Rotation of the box.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction,Quaternion orientation, 
            float maxDistance = float.PositiveInfinity,int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.BoxCastNonAlloc(center, halfExtents, direction, RaycastBuffer, orientation, 
                maxDistance,layerMask,queryTriggerInteraction);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region CapsuleCast ////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to preform a capsule cast using the allocated buffers.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>The result of the box cast.</returns>
        public IEnumerable<RaycastHit> CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, 
            bool sortByDistance = false, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            DoCapsuleCast(point1,point2,radius,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) yield return RaycastBuffer[i];
        }
        
        /// <summary>
        /// This method is used to get all of the <see cref="MonoBehaviour"/> of the given type.
        /// <see cref="T"/> of all the <see cref="GameObject"/> hit by the capsule cast.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <typeparam name="T">The type of <see cref="MonoBehaviour"/> you want to return from the capsule cast.</typeparam>
        /// <returns>All the <see cref="T"/> <see cref="MonoBehaviour"/> from the capsule cast. </returns>
        public IEnumerable<T> FilteredCapsuleCast<T>(Vector3 point1, Vector3 point2, float radius, Vector3 direction, 
            bool sortByDistance = false, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            DoCapsuleCast(point1,point2,radius,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) SortResultsByDistance();
            //return the values
            for(var i = 0; i < HitSize; i++) 
                foreach(var component in RaycastBuffer[i].transform.GetComponents<T>())
                    yield return component;
        }

        /// <summary>
        /// Casts a capsule along the direction and store hits in the buffer.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        public void DoCapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, 
            float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, RaycastBuffer, maxDistance, layerMask,
                queryTriggerInteraction);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region SortResults ////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to sort the result of
        /// a raycast by the distance of the GameObject.
        /// </summary>
        public void SortResultsByDistance() {
            for(var i = 0; i < HitSize; i++) _distanceBuffer[i] = RaycastBuffer[i].distance;
            Array.Sort(_distanceBuffer,RaycastBuffer,0,HitSize);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    /// <summary>
    /// This class is used to cast for specific <see cref="MonoBehaviour"/>s attached to hit <see cref="GameObject"/>s.
    /// </summary>
    /// <typeparam name="T">The type of component that the casting should return.</typeparam>
    public class BufferedCaster<T> : IEnumerator<T>, IEnumerable<T> where T: MonoBehaviour {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private int _index = -1;
        private readonly BufferedCaster _caster;
        private readonly T[] _values;
        private int _maxId = -1;
        private readonly int _allowedMaxId;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public T Current => _index<0||_index>_maxId?null:_values[_index];
        /// <inheritdoc />
        object IEnumerator.Current => Current;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new buffered caster.
        /// </summary>
        /// <param name="bufferSize">The size of both the result array and the hit array.</param>
        public BufferedCaster(int bufferSize) {
            _allowedMaxId = bufferSize-1;
            _caster = new BufferedCaster(bufferSize);
            _values = new T[bufferSize];
        }

        /// <summary>
        /// This constructor is used to create a new buffered caster.
        /// </summary>
        /// <param name="bufferSize">The size of the buffer.</param>
        /// <param name="maxResults">The size of the result array.</param>
        public BufferedCaster(int bufferSize, int maxResults) {
            _allowedMaxId = maxResults-1;
            _caster = new BufferedCaster(bufferSize);
            _values = new T[maxResults];
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region RayCast ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the ray cast.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the ray cast. </returns>
        public BufferedCaster<T> Raycast(Ray ray, bool sortByDistance = false, float maxDistance = Mathf.Infinity, 
            int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            _caster.DoRaycast(ray,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            //return the values
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }

        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the ray cast.
        /// </summary>
        /// <param name="origin">The starting point and direction of the ray.</param>
        /// <param name="direction">The direction of the ray.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max distance the rayhit is allowed to be from the start of the ray.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a ray.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the ray cast. </returns>
        public BufferedCaster<T> Raycast(Vector3 origin, Vector3 direction, bool sortByDistance = false, 
            float maxDistance = Mathf.Infinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do raycast
            _caster.DoRaycast(origin,direction,maxDistance,layerMask, queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            //return the values
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region SphereCast /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the sphere cast.
        /// </summary>
        /// <param name="origin">The center of the sphere at the start of the sweep.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The direction in which to sweep the sphere.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the sphere cast. </returns>
        public BufferedCaster<T> SphereCast(Vector3 origin, float radius, Vector3 direction,
            bool sortByDistance = false, float maxDistance = Mathf.Infinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            _caster.DoSphereCast(origin,radius,direction,maxDistance,layerMask, queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            //return the values
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }

        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the sphere cast.
        /// </summary>
        /// <param name="ray">The starting point and direction of the ray into which the sphere sweep is cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a sphere.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the sphere cast. </returns>
        public BufferedCaster<T> SphereCast(Ray ray, float radius, bool sortByDistance = false, 
            float maxDistance = Mathf.Infinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            _caster.DoSphereCast(ray,radius,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            //return the values
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region BoxCast ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the box cast.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the box cast. </returns>
        public BufferedCaster<T> BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, 
            bool sortByDistance, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            _caster.DoBoxCast(center,halfExtents,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }
        
        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the box cast.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="halfExtents">Half the size of the box in each dimension.</param>
        /// <param name="direction">The direction in which to cast the box.</param>
        /// <param name="orientation">Rotation of the box.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the cast.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the box cast. </returns>
        public BufferedCaster<T> BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, 
            Quaternion orientation, bool sortByDistance, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            _caster.DoBoxCast(center,halfExtents,direction,orientation,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            //return the values
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region CapsuleCast ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get all of the matching <see cref="MonoBehaviour"/>s of all the
        /// <see cref="GameObject"/> hit by the capsule cast.
        /// </summary>
        /// <param name="point1">The center of the sphere at the start of the capsule.</param>
        /// <param name="point2">The center of the sphere at the end of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="direction">The direction into which to sweep the capsule.</param>
        /// <param name="sortByDistance">If true the results will be sorted by distance,
        /// otherwise the results will not be sorted.</param>
        /// <param name="maxDistance">The max length of the sweep.</param>
        /// <param name="layerMask">A that is used to selectively ignore colliders when casting a capsule.</param>
        /// <param name="queryTriggerInteraction">Specifies whether this query should hit Triggers.</param>
        /// <returns>All the matching <see cref="MonoBehaviour"/>s from the capsule cast. </returns>
        public BufferedCaster<T> CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, 
            bool sortByDistance = false, float maxDistance = float.PositiveInfinity, int layerMask = -5, 
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            //do the sphere cast
            _caster.DoCapsuleCast(point1,point2,radius,direction,maxDistance,layerMask,queryTriggerInteraction);
            //sort raycast
            if(sortByDistance) _caster.SortResultsByDistance();
            //return the values
            Reset();
            _maxId = -1;
            for(var i = 0; i < _caster.HitSize; i++) {
                foreach(var component in _caster.RaycastBuffer[i].transform.GetComponents<T>()) {
                    if(++_maxId > _allowedMaxId) return this;
                    _values[_maxId] = component;
                }
            }
            return this;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Interface Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool MoveNext() => ++_index > _maxId; 
        /// <inheritdoc />
        public void Reset() => _index = -1;
        /// <inheritdoc />
        public void Dispose() => Reset();
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() { return this; }
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() { return this; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}