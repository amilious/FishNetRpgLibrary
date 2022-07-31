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

namespace Amilious.FunctionGraph {

    /// <summary>
    /// DO NOT IMPLEMENT THIS INTERFACE ONLY USE <see cref="PortInfo{T}"/>.
    /// </summary>
    public interface IPortInfo {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the type of the port.
        /// </summary>
        public Type Type { get; }
        
        /// <summary>
        /// This property contains the name of the port.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// This property indicates whether or not the port allows multiple connections.
        /// </summary>
        public bool AllowMultiple { get; }
        
        /// <summary>
        /// This property indicates whether the port should be horizontal or vertical.
        /// </summary>
        public bool Horizontal { get; }
        
        /// <summary>
        /// This property contains the port's tooltip.
        /// </summary>
        string Tooltip { get; }
        
        /// <summary>
        /// This property is used to get the port's index.
        /// </summary>
        public int Index { get; }
        
        /// <summary>
        /// This property is used to indicate whether or not the port is a loop start or end point.
        /// </summary>
        public bool IsLoopPort { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set the port's index.
        /// </summary>
        /// <param name="index">The index that you want to set.</param>
        public void SetIndex(int index);
        
        /// <summary>
        /// This method is used to try get the value of an output port.
        /// </summary>
        /// <param name="id">A calculation id.</param>
        /// <param name="result">The resulting value.</param>
        /// <typeparam name="T">The type of the value.  This should match the port type.</typeparam>
        /// <returns>True if able to get a value, otherwise false.</returns>
        public bool TryGetResult<T>(CalculationId id, out T result) {
            if(this is PortInfo<T> casted) {
                var function = casted.OutputFunction;
                if(function == null) {
                    result = default(T);
                    return false;
                }
                result = function.Invoke(id);
                return true;
            }else {
                result = default(T);
                return false;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
    /// <summary>
    /// This class is used to hold port information.
    /// </summary>
    /// <typeparam name="T">The value type of the port.</typeparam>
    public class PortInfo<T> : IPortInfo {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public Type Type { get; }
        
        /// <inheritdoc />
        public string Name { get; }
        
        /// <inheritdoc />
        public bool AllowMultiple { get; }
        
        /// <inheritdoc />
        public bool Horizontal { get; }
        
        /// <inheritdoc />
        public string Tooltip { get; set; }
        
        /// <inheritdoc />
        public int Index { get; private set; }

        /// <inheritdoc />
        public bool IsLoopPort { get; private set; }
        
        /// <inheritdoc />
        public void SetIndex(int index) => Index = index;
        
        /// <summary>
        /// This property is used to get the output function.
        /// </summary>
        public Func<CalculationId,T> OutputFunction { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
      
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to make an input port.
        /// </summary>
        /// <param name="name">The name of the port.</param>
        /// <param name="allowMultiple">Whether this port allows multiple connections.</param>
        /// <param name="horizontal">Whether this port is horizontal.</param>
        public PortInfo(string name, bool allowMultiple = false, bool horizontal = true) {
            Type = typeof(T);
            Name = name;
            AllowMultiple = allowMultiple;
            Horizontal = horizontal;
            OutputFunction = null;
        }

        /// <summary>
        /// This constructor is used to make an output port.
        /// </summary>
        /// <param name="name">The name of the port.</param>
        /// <param name="outputFunction">The method that should be called to get the value of this port.</param>
        /// <param name="allowMultiple">Whether this port allows multiple connections.</param>
        /// <param name="horizontal">Whether this port is horizontal.</param>
        public PortInfo(string name,Func<CalculationId,T> outputFunction, bool allowMultiple = true, 
            bool horizontal = true) {
            Type = typeof(T);
            Name = name;
            AllowMultiple = allowMultiple;
            Horizontal = horizontal;
            OutputFunction = outputFunction;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set the tooltip in a chain.
        /// </summary>
        /// <param name="tooltip">The text you want displayed as a tooltip.</param>
        /// <returns>This <see cref="PortInfo{T}"/></returns>
        public PortInfo<T> SetTooltip(string tooltip) { 
            Tooltip = tooltip;
            return this;
        }

        /// <summary>
        /// This method is used to mark the port as a loop port in a chain.
        /// </summary>
        /// <returns>This <see cref="PortInfo{T}"/></returns>
        public PortInfo<T> MarkLoop() {
            IsLoopPort = true;
            return this;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}