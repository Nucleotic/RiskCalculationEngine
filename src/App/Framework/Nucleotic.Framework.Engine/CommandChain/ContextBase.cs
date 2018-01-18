using System;
using Nucleotic.Framework.Engine.CommandChain.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Nucleotic.Framework.Engine.CommandChain
{
    /// <summary>
    /// Base class for IContext implementations. Simple hashmap.
    /// </summary>
    /// <seealso cref="Hashtable" />
    /// <seealso cref="IContext" />
    public class ContextBase : Hashtable, IContext
    {
        public ContextBase()
        {
            Errors = new List<Exception>();
        }

        public List<Exception> Errors { get; set; }
    }
}