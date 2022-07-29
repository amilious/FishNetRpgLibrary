namespace Amilious.Core.Attributes {
    
    public class DynamicLabelAttribute : AmiliousModifierAttribute {

        public string NameOfLabelField { get; }
        
        public DynamicLabelAttribute(string nameOfLabelField) {
            NameOfLabelField = nameOfLabelField;
        }
        
    }
    
}