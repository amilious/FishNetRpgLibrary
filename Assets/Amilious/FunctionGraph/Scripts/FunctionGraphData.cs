using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Amilious.FunctionGraph {
    public class FunctionGraphData : ScriptableObject {

        [SerializeField] public Vector3 position = Vector3.zero;
        [SerializeField] public Vector3 scale = Vector3.one;
        [SerializeField] public List<FunctionGroup> groups = new List<FunctionGroup>();
        
        

        public FunctionGroup GroupFromId(string id) {
            return groups.FirstOrDefault(x => x.id == id);
        }

        public bool RemoveGroup(string id) {
            return groups.RemoveAll(x => x.id == id) > 0;
        }

    }
}