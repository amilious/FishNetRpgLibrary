using System;
using System.Collections.Generic;

namespace Amilious.FunctionGraph {
    
    [Serializable]
    public class FunctionGroup {

        public string id;
        public string title;
        public List<string> nodeIds = new List<string>();

    }
}