namespace Amilious.FishNetRpg.Statistics {
    public class StatData {
        public int Level;
        public int BaseValue;
        public int Value;

        public StatData() { }

        public StatData(int level, int baseValue, int value) {
            Level = level;
            BaseValue = baseValue;
            Value = value;
        }
    }
}