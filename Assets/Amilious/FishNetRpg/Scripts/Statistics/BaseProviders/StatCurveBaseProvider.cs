using UnityEngine;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {
    
    [CreateAssetMenu(fileName = "NewCurveBaseProvider",
        menuName = FishNetRpg.STATS_MENU_ROOT + "Curve Base Value Provider", order = 46)]
    public class StatCurveBaseProvider : StatBaseValueProvider {

        [SerializeField]private int cap;
        [SerializeField]private int minimum;
        [SerializeField]private AnimationCurve curve = GenerateDefault();

        /// <inheritdoc />
        public override int GetMinimum(int level) => minimum;

        /// <inheritdoc />
        public override int GetCap(int level) => cap;

        /// <inheritdoc />
        public override int BaseValue(int level) => Mathf.RoundToInt(curve.Evaluate(level));
        
        public static AnimationCurve GenerateDefault() {
            var ac = new AnimationCurve();
            ac.AddKey(new Keyframe(0, 100));
            ac.AddKey(new Keyframe(100, 100000));
            ac.postWrapMode = WrapMode.ClampForever;
            ac.preWrapMode = WrapMode.ClampForever;
            return ac;
        }
    }
}