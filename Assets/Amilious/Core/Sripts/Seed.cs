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
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Security.Cryptography;

namespace Amilious.Core {
    
    /// <summary>
    /// This class is used to represent a seed for procedural generation
    /// </summary>
    [Serializable]
    public class Seed {
        
        #region Private Instance Variables /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This value is used to store the random state.
        /// </summary>
        private Random.State _state;
        
        /// <summary>
        /// This random is used to generate integer values when needed.
        /// </summary>
        private readonly System.Random _random;
        
        /// <summary>
        /// This random is used to generate random seeds.
        /// </summary>
        private static readonly System.Random StaticRandom = new ();

        #endregion
        

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// If set to true, the random state will be restored before a value is generated and saved after generation.
        /// This only needs to be true if you are using multiple seeds within a project at the same time.
        /// </summary>
        public bool AutoSaveAndRestoreState { get; set; } = false;

        /// <summary>
        /// This property contains the string name of the seed.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// This property contains the int value of the seed.
        /// </summary>
        public int SeedIntValue { get; }
        
        /// <summary>
        /// This property contains the long value of the seed.
        /// </summary>
        public long SeedLongValue { get; }
        
        #endregion
        
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to generate a random seed.
        /// </summary>
        public Seed() {
            Name = StaticRandom.Next(int.MinValue, int.MaxValue).ToString();
            SeedIntValue = GetSeedInt(Name);
            SeedLongValue = GetSeedLong(Name);
            Random.InitState(SeedIntValue);
            _state = Random.state;
            _random = new System.Random(SeedIntValue);
        }

        /// <summary>
        /// This constructor is used to create a new copy of the passed seed.
        /// </summary>
        /// <param name="seed">The seed that you want to copy.</param>
        public Seed(Seed seed) {
            Name = seed.Name;
            SeedIntValue = seed.SeedIntValue;
            SeedLongValue = seed.SeedLongValue;
            Random.InitState(SeedIntValue);
            _state = Random.state;
            _random = new System.Random(SeedIntValue);
            _state = seed._state;
        }

        /// <summary>
        /// This constructor is used to create a seed using the passed string name.
        /// </summary>
        /// <param name="name">The string to use as the seed.</param>
        public Seed(string name) {
            Name = string.IsNullOrWhiteSpace(name) ? StaticRandom.Next(int.MinValue, int.MaxValue).ToString() : name;
            SeedIntValue = GetSeedInt(Name);
            SeedLongValue = GetSeedLong(Name);
            Random.InitState(SeedIntValue);
            _state = Random.state;
            _random = new System.Random(SeedIntValue);
        }

        /// <summary>
        /// This constructor is used to create a seed using the passed int value.
        /// </summary>
        /// <param name="value">The int to use as the seed.</param>
        public Seed(int value) {
            Name = value.ToString();
            SeedIntValue = GetSeedInt(Name);
            SeedLongValue = GetSeedLong(Name);
            Random.InitState(SeedIntValue);
            _state = Random.state;
            _random = new System.Random(SeedIntValue);
        }

        /// <summary>
        /// This constructor is used to create a seed using the passed long value.
        /// </summary>
        /// <param name="value">The long to use as the seed.</param>
        public Seed(long value) {
            Name = value.ToString();
            SeedIntValue = GetSeedInt(Name);
            SeedLongValue = GetSeedLong(Name);
            Random.InitState(SeedIntValue);
            _state = Random.state;
            _random = new System.Random(SeedIntValue);
        }
        
        #endregion
        
        
        #region Random Value Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the next random float.
        /// </summary>
        /// <returns>A float in the range of 0 to 1.</returns>
        public float NextFloat() {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return Random.value;
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to get the next float in the given range.
        /// </summary>
        /// <param name="minInclusive">The minimum possible float.</param>
        /// <param name="maxInclusive">The maximum possible float.</param>
        /// <returns>A float within the given range.</returns>
        public float NextRange(float minInclusive, float maxInclusive) {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return Random.Range(minInclusive,maxInclusive);
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to get the next float in the given range.
        /// </summary>
        /// <param name="minMaxInclusive">The minimum and maximum possible float where x is the minimum and y
        /// is the maximum.</param>
        /// <returns>A float within the given range.</returns>
        public float NextRange(Vector2 minMaxInclusive) {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return Random.Range(minMaxInclusive.x,minMaxInclusive.y);
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }
        
        /// <summary>
        /// This method is used to get the next true or false value based on the given probability.
        /// </summary>
        /// <param name="probability">The probability that the value will be true.</param>
        /// <returns>A bool based on the passed probability.</returns>
        public bool NextBool(float probability = .5f) {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return Random.value <= probability;
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to get the next rotational value.
        /// </summary>
        /// <returns>A random rotational value.</returns>
        public Quaternion NextRotation() {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return Random.rotation;
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }
        
        /// <summary>
        /// This method is used to get the next rotational value with uniform distribution.
        /// </summary>
        /// <returns>A random rotational value with uniform distribution.</returns>
        public Quaternion NextRotationUniform() {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return Random.rotationUniform;
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to return a <see cref="Vector3"/> in the range of Vector3.zero(0,0,0)
        /// and Vector3.one(1,1,1)
        /// </summary>
        /// <returns>A Vector3 in the range of Vector3.zero(0,0,0) and Vector3.one(1,1,1).</returns>
        public Vector3 NextVector3() {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return new Vector3(Random.value, Random.value, Random.value);
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to get the next <see cref="Vector3"/> in the given range.
        /// </summary>
        /// <param name="minInclusive">The minimum possible x, y, and z values.</param>
        /// <param name="maxInclusive">The maximum possible x, y, and z values.</param>
        /// <returns>A Vector3 in the given range.</returns>
        public Vector3 NextVector3Range(Vector3 minInclusive, Vector3 maxInclusive) {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return new Vector3(
                    Random.Range(minInclusive.x, maxInclusive.x), 
                    Random.Range(minInclusive.y, maxInclusive.y), 
                    Random.Range(minInclusive.z, maxInclusive.z));
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to return a <see cref="Vector2"/> in the range of Vector2.zero(0,0) and Vector2.one(1,1)
        /// </summary>
        /// <returns>A Vector2 in the range of Vector2.zero(0,0) and Vector2.one(1,1).</returns>
        public Vector2 NextVector2() {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return new Vector2(Random.value, Random.value);
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }
        
        /// <summary>
        /// This method is used to get the next <see cref="Vector2"/> in the given range.
        /// </summary>
        /// <param name="minInclusive">The minimum possible x and y values.</param>
        /// <param name="maxInclusive">The maximum possible x and y values.</param>
        /// <returns>A Vector2 in the given range.</returns>
        public Vector2 NextVector2Range(Vector2 minInclusive, Vector2 maxInclusive) {
            try { if(AutoSaveAndRestoreState) RestoreState();
                return new Vector3(
                    Random.Range(minInclusive.x, maxInclusive.x), 
                    Random.Range(minInclusive.y, maxInclusive.y));
            } finally {if(AutoSaveAndRestoreState) SaveState(); }
        }

        /// <summary>
        /// This method is used to pick a random value from a list.
        /// </summary>
        /// <param name="collection">The list you want to select a value from.</param>
        /// <typeparam name="T">The list's item type.</typeparam>
        /// <returns>A random value from the list.</returns>
        public T NextValue<T>(IList<T> collection) {
            return collection == null ? default(T) : collection[_random.Next(collection.Count)];
        }

        /// <summary>
        /// This method is used to pick a random value from an array.
        /// </summary>
        /// <param name="collection">The array you want to select a value from.</param>
        /// <typeparam name="T">The array's item type.</typeparam>
        /// <returns>A random value from the array.</returns>
        public T NextValue<T>(T[] collection) {
            return collection == null ? default(T) : collection[_random.Next(collection.Length)];
        }

        /// <summary>
        /// This method is used to pick a random index from a list.
        /// </summary>
        /// <param name="collection">The list you want to select an index from.</param>
        /// <typeparam name="T">The list's item type.</typeparam>
        /// <returns>A random index for the given list.</returns>
        public int NextIndex<T>(IList<T> collection) => collection==null?-1:_random.Next(collection.Count);

        /// <summary>
        /// This method is used to pick a random index from an array.
        /// </summary>
        /// <param name="collection">The array you want to select an index from.</param>
        /// <typeparam name="T">The array's item type.</typeparam>
        /// <returns>A random index for the given array.</returns>
        public int NextIndex<T>(T[] collection) => collection==null?-1:_random.Next(collection.Length);

        #endregion
        
        
        #region Override Methods ///////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public override string ToString() => Name;
        
        /// <inheritdoc />
        public override int GetHashCode() => SeedIntValue;

        /// <inheritdoc />
        public override bool Equals(object obj) {
            return obj switch {
                string objString => Name.Equals(objString),
                int objInt => objInt == SeedIntValue,
                long objLong => objLong == SeedLongValue,
                Seed objSeed => objSeed.SeedIntValue == SeedIntValue,
                _ => false
            };
        }

        #endregion
        
        
        #region Other Public Methods ///////////////////////////////////////////////////////////////////////////////////
        
        public void SaveState() => _state = Random.state;

        public void RestoreState() => Random.state = _state;

        public bool Equals(Seed other) {
            return SeedIntValue == other.SeedIntValue;
        }
        
        #endregion
        

        #region Static Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the seed's int value based on the given string.
        /// </summary>
        /// <param name="seed">The seed string value.</param>
        /// <returns>The seed's int value.</returns>
        private static int GetSeedInt(string seed) {
            if(int.TryParse(seed, out var value)) return value;
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)),0);
            return hash;
        }
        
        /// <summary>
        /// This method is used to get the seed's long value based on the given string.
        /// </summary>
        /// <param name="seed">The seed string value.</param>
        /// <returns>The seed's long value.</returns>
        private static long GetSeedLong(string seed) {
            if(long.TryParse(seed, out var value)) return value;
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt64(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)),0);
            return hash;
        }
        
        /// <summary>
        /// This method is used to auto-cast a string to a seed.
        /// </summary>
        /// <param name="name">The string that you want to cast to a seed.</param>
        /// <returns>The seed created by the casted string.</returns>
        public static implicit operator Seed(string name) => new Seed(name);

        /// <summary>
        /// This method is used to auto-cast an int to a seed.
        /// </summary>
        /// <param name="value">The int that you want to cast to a seed.</param>
        /// <returns>The seed created by the casted int.</returns>
        public static implicit operator Seed(int value) => new Seed(value);

        /// <summary>
        /// This method is used to auto-cast a long to a seed.
        /// </summary>
        /// <param name="value">The long that you want to cast to a seed.</param>
        /// <returns>The seed created by the casted long.</returns>
        public static implicit operator Seed(long value) => new Seed(value);

        /// <summary>
        /// This method is used to auto-cast a seed to a string.
        /// </summary>
        /// <param name="seed">The seed that you want to cast to a string.</param>
        /// <returns>The string value of the seed.</returns>
        public static explicit operator string(Seed seed) => seed.Name;
        
        /// <summary>
        /// This method is used to auto-cast a seed to an int.
        /// </summary>
        /// <param name="seed">The seed that you want to cast to an int.</param>
        /// <returns>The int value of the seed.</returns>
        public static explicit operator int(Seed seed) => seed.SeedIntValue;

        /// <summary>
        /// This method is used to auto-cast a seed to a long.
        /// </summary>
        /// <param name="seed">The seed that you want to cast to a long.</param>
        /// <returns>The long value of the seed.</returns>
        public static explicit operator long(Seed seed) => seed.SeedLongValue;

        /// <summary>
        /// This method is used to check if a seed is equal to another value.
        /// </summary>
        /// <param name="a">The seed you are comparing.</param>
        /// <param name="b">The object that you are comparing to the seed.</param>
        /// <returns>True if the seed is equal to the compared value, otherwise false.</returns>
        public static bool operator == (Seed a, object b) => a==null?b==null:a.Equals(b);
        
        /// <summary>
        /// This method is used to check if a seed is not equal to another value.
        /// </summary>
        /// <param name="a">The seed that you are comparing.</param>
        /// <param name="b">The object that you are comparing to the seed.</param>
        /// <returns>True if the seed is not equal to the compared value, otherwise false.</returns>
        public static bool operator != (Seed a, object b) => a==null?b!=null:!a.Equals(b);
        
        #endregion

    }
    
}