using System;

namespace Chizl.ColorExtension
{
    public class ToneRule
    {
        private ToneRule() { IsEmpty = true; }

        public ToneRule(int ruleNo, Func<double, double, bool> condition, string modifier, string rulesDisplay)
        {
            RuleNo = ruleNo;
            Condition = condition;
            Modifier = modifier;
            RulesDisplay = rulesDisplay;
        }

        /// <summary>
        /// Empty Validation only set to true when not fully initialized.
        /// </summary>
        public bool IsEmpty { get; } = false;
        /// <summary>
        /// Rule #
        /// </summary>
        public int RuleNo { get; }
        /// <summary>
        /// Condition for Modifier
        /// </summary>
        public Func<double, double, bool> Condition { get; }
        /// <summary>
        /// Response text.
        /// </summary>
        public string Modifier { get; }
        /// <summary>
        /// Rules used
        /// </summary>
        public string RulesDisplay { get; }
        /// <summary>
        /// Saturation, is set during response with value found within range of this Condition.
        /// </summary>
        public double Saturation { get; internal set; }
        /// <summary>
        /// Value, is set during response with value found within range of this Condition.
        /// </summary>
        public double Value { get; internal set; }
        /// <summary>
        /// Used when nothing else is valid.
        /// </summary>
        public static ToneRule Empty { get { return new ToneRule(); } }


        #region Public Override Methods
        public override bool Equals(object obj) => obj is ToneRule other && Condition.Equals(other.Condition);
        public override int GetHashCode() => Condition.GetHashCode();
        public override string ToString() => Modifier;
        #endregion
    }
}
