namespace Nucleotic.Framework.Logging.LogEntries
{
    public interface ICodeLocation
    {
        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        string ClassName { get; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        string FileName { get; }

        /// <summary>
        /// Gets the line number.
        /// </summary>
        /// <value>The line number.</value>
        int LineNumber { get; }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        string MethodName { get; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="CodeLocation"/>.
        /// The format is &lt;FileName&gt;(&lt;LineNumber&gt;):&lt;ClassName&gt;.&lt;MethodName&gt;
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="CodeLocation"/>.
        /// </returns>
        string ToString();
    }
}