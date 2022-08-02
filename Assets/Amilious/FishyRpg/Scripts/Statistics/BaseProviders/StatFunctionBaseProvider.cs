/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEngine;
using Amilious.Core;
using Amilious.FunctionGraph;
using System.Collections.Generic;

namespace Amilious.FishyRpg.Statistics.BaseProviders {

    /// <summary>
    /// This class uses <see cref="IFunctionProvider"/> to use a function graph as a <see cref="StatBaseValueProvider"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFunctionBaseProvider",
        menuName = FishyRpg.STATS_MENU_ROOT + "Function Base Value Provider", order = FishyRpg.STAT_START+103)]
    public class StatFunctionBaseProvider : StatBaseValueProvider, IFunctionProvider {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [HideInInspector, SerializeField] private FunctionBaseProviderResult resultNode;
        [HideInInspector, SerializeField] private FunctionBaseProviderSource sourceNode;
        [HideInInspector, SerializeField] private List<FunctionNode> nodes = new List<FunctionNode>();
        [HideInInspector, SerializeField] private FunctionGraphData graphData;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        FunctionGraphData IFunctionProvider.GetGraphData() => graphData;
        
        /// <inheritdoc />
        void IFunctionProvider.SetGraphData(FunctionGraphData data) => graphData = data;
        
        /// <inheritdoc />
        bool IFunctionProvider.Initialized { get; set; }
        
        /// <inheritdoc />
        public FunctionNode GetInputNode => sourceNode;
        
        /// <inheritdoc />
        public FunctionNode GetResultNode => resultNode;
        
        /// <inheritdoc />
        public List<FunctionNode> GetNodes() => nodes;
        
        /// <inheritdoc />
        public AmiliousScriptableObject FunctionProvider => this;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public List<FunctionNode> InitializeInputAndOutputs() {
            //do no initialize if already done
            if(sourceNode && resultNode) return new List<FunctionNode>();
            //create list
            var nodeList = new List<FunctionNode>();
            //add result
            resultNode = CreateInstance<FunctionBaseProviderResult>();
            resultNode.name = "Result";
            resultNode.SetPositionWithoutSaving(Vector2.right*100);
            resultNode.SetLabel("Base Value", "Minimum", "Cap");
            nodeList.Add(resultNode);
            //add input
            sourceNode = CreateInstance<FunctionBaseProviderSource>();
            sourceNode.name = "Input";
            sourceNode.SetLabel("level");
            nodeList.Add(sourceNode);
            //return the list
            return nodeList;
        }

        /// <inheritdoc />
        public void AfterInitialization(IFunctionProvider provider) {
            var con = new Connection(resultNode, 0, sourceNode, 0);
            provider.AddConnection(con);
        }

        /// <inheritdoc />
        public override int GetMinimum(int level) {
            var calculationId = new CalculationId();
            sourceNode.SetValue(level);
            return resultNode.GetResult2(calculationId);
        }

        /// <inheritdoc />
        public override int GetCap(int level) {
            var calculationId = new CalculationId();
            sourceNode.SetValue(level);
            return resultNode.GetResult3(calculationId);
        }

        /// <inheritdoc />
        public override int BaseValue(int level) {
            var calculationId = new CalculationId();
            sourceNode.SetValue(level);
            return Mathf.RoundToInt(resultNode.GetResult1(calculationId));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}