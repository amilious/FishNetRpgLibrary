using UnityEngine;
using Amilious.FunctionGraph;
using System.Collections.Generic;


namespace Amilious.FishNetRpg.Statistics.BaseProviders {

    [CreateAssetMenu(fileName = "NewFunctionBaseProvider",
        menuName = FishNetRpg.STATS_MENU_ROOT + "Function Base Value Provider", order = 47)]
    public class StatFunctionBaseProvider : StatBaseValueProvider, IFunctionProvider {

        [HideInInspector, SerializeField] private FunctionBaseProviderResult resultNode;
        [HideInInspector, SerializeField] private FunctionBaseProviderInput inputNode;
        [HideInInspector, SerializeField] private List<FunctionNode> nodes = new List<FunctionNode>();
        [HideInInspector, SerializeField] private FunctionGraphData graphData;

        FunctionGraphData IFunctionProvider.GetGraphData() => graphData;
        void IFunctionProvider.SetGraphData(FunctionGraphData data) => graphData = data;
        bool IFunctionProvider.Initialized { get; set; }
        public FunctionNode GetInputNode => inputNode;
        public FunctionNode GetResultNode => resultNode;
        public List<FunctionNode> GetNodes() => nodes;
        public ScriptableObject FunctionProvider => this;

        public List<FunctionNode> InitializeInputAndOutputs() {
            //do no initialize if already done
            if(inputNode && resultNode) return new List<FunctionNode>();
            //create list
            var nodeList = new List<FunctionNode>();
            //add result
            resultNode = CreateInstance<FunctionBaseProviderResult>();
            resultNode.name = "Result";
            resultNode.SetPositionWithoutSaving(Vector2.right*100);
            resultNode.SetLabel("Base Value", "Minimum", "Cap");
            nodeList.Add(resultNode);
            //add input
            inputNode = CreateInstance<FunctionBaseProviderInput>();
            inputNode.name = "Input";
            inputNode.label="level";
            nodeList.Add(inputNode);
            //return the list
            return nodeList;
        }

        public void AfterInitialization(IFunctionProvider provider) {
            var con = new Connection(resultNode, 0, inputNode, 0);
            provider.AddConnection(con);
        }

        public override int GetMinimum(int level) {
            var calculationId = new CalculationId();
            inputNode.SetValue(level);
            return resultNode.GetResult2(calculationId);
        }

        public override int GetCap(int level) {
            var calculationId = new CalculationId();
            inputNode.SetValue(level);
            return resultNode.GetResult3(calculationId);
        }

        public override int BaseValue(int level) {
            var calculationId = new CalculationId();
            inputNode.SetValue(level);
            return Mathf.RoundToInt(resultNode.GetResult1(calculationId));
        }
    }

}