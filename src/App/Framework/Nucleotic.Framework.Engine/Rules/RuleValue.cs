using System.Collections.Generic;

namespace Nucleotic.Framework.Engine.Rules
{
    /// <summary>
    /// The generic Rule value defined by the defined expression type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RuleValue<T>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        public List<Rule> Rules { get; set; }
    }

    /// <summary>
    /// Rule value string representing the string value of a compiled rule.
    /// </summary>
    /// <seealso cref="string" />
    public class RuleValueString : RuleValue<string> { }
}