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
using Amilious.FunctionGraph;
using System.Collections.Generic;
using Amilious.Core;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {

    [CreateAssetMenu(fileName = "NewFunctionBaseProvider",
        menuName = FishNetRpg.STATS_MENU_ROOT + "Function Base Value Provider", order = 47)]
    public class StatFunctionBaseProvider : StatBaseValueProvider, IFunctionProvider {

        [HideInInspector, SerializeField] private FunctionBaseProviderResult resultNode;
        [HideInInspector, SerializeField] private FunctionBaseProviderSource sourceNode;
        [HideInInspector, SerializeField] private List<FunctionNode> nodes = new List<FunctionNode>();
        [HideInInspector, SerializeField] private FunctionGraphData graphData;

        FunctionGraphData IFunctionProvider.GetGraphData() => graphData;
        void IFunctionProvider.SetGraphData(FunctionGraphData data) => graphData = data;
        bool IFunctionProvider.Initialized { get; set; }
        public FunctionNode GetInputNode => sourceNode;
        public FunctionNode GetResultNode => resultNode;
        public List<FunctionNode> GetNodes() => nodes;
        public AmiliousScriptableObject FunctionProvider => this;

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
    }

}