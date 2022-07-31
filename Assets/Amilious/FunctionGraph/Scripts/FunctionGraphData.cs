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

using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.FunctionGraph {
    
    /// <summary>
    /// This class is used to store information about the graph.
    /// </summary>
    public class FunctionGraphData : ScriptableObject {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] public Vector3 position = Vector3.zero;
        [SerializeField] public Vector3 scale = Vector3.one;
        [SerializeField] public List<FunctionGroup> groups = new List<FunctionGroup>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is sued to get the group for the given id.
        /// </summary>
        /// <param name="id">The guid of the group.</param>
        /// <returns>The group with the give id.</returns>
        public FunctionGroup GroupFromId(string id) {
            return groups.FirstOrDefault(x => x.id == id);
        }

        /// <summary>
        /// This method is used to remove the group that has the given id.
        /// </summary>
        /// <param name="id">The id of the group that you want to remove.</param>
        /// <returns>True if the node existed and was removed, otherwise false.</returns>
        public bool RemoveGroup(string id) {
            return groups.RemoveAll(x => x.id == id) > 0;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}