using System;

namespace Chizl.ColorExtension
{
    public class CmykRule
    {
        private CmykRule() { IsEmpty = true; }

        public CmykRule(int ruleNo, Func<double, double, double, double, bool> condition, string modifier, string rulesDisplay )
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
        public Func<double, double, double, double, bool> Condition { get; set; }
        /// <summary>
        /// Response text.
        /// </summary>
        public string Modifier { get; }
        /// <summary>
        /// Rules used
        /// </summary>
        public string RulesDisplay { get; }
        /// <summary>
        /// Cyan, is set during response with value found within range of this Condition.
        /// </summary>
        public double Cyan { get; internal set; }
        /// <summary>
        /// Magenta, is set during response with value found within range of this Condition.
        /// </summary>
        public double Magenta { get; internal set; }
        /// <summary>
        /// Yellow, is set during response with value found within range of this Condition.
        /// </summary>
        public double Yellow { get; internal set; }
        /// <summary>
        /// Key, is set during response with value found within range of this Condition.
        /// </summary>
        public double Key { get; internal set; }
        /// <summary>
        /// Black aka Key, will have the same value as Key.
        /// </summary>
        public double Black { get { return this.Key; } }
        /// <summary>
        /// Used when nothing else is valid.
        /// </summary>
        public static CmykRule Empty { get { return new CmykRule(); } }


        #region Public Override Methods
        public override bool Equals(object obj) => obj is CmykRule other && Condition.Equals(other.Condition);
        public override int GetHashCode() => Condition.GetHashCode();
        public override string ToString() => Modifier;
        #endregion
    }
}
