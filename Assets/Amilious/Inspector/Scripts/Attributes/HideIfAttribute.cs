namespace Amilious.Inspector.Attributes {
    public class HideIfAttribute : AmiliousModifierAttribute {
        
        public string PropertyName { get; }

        public bool SetValue { get; }
        public object Value { get; }

        public HideIfAttribute(string propertyName) {
            PropertyName = propertyName;
        }
        
        public HideIfAttribute(string propertyName, object value){
            PropertyName = propertyName;
            Value = value;
            SetValue = true;
        }

        public bool Validate<T>(T value) {
            if(SetValue) return Value is T casted && casted.Equals(value);
            if(value is bool boolValue) return boolValue;
            if(default(T) == null) return value != null;
            return false;
        }

        public bool Validate(object value) {
            if(SetValue) return value.Equals(Value);
            if(value is bool boolValue) return boolValue;
            return value != null;
        }

        public bool ValidateEnumValue(int index, int flag) {
            if(!Value.GetType().IsEnum) return false;
            var casted = (int)Value;
            return casted == index || casted == flag;
        }
        
    }
}