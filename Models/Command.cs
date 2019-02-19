using System;
using System.Collections.Specialized;

namespace Models
{
    [Serializable]
    public class RestfulCommand
    {
        public string Uri { get; set; }
        public HttpVerb Verb { get; set; }

        /// <summary>
        /// Serialize your function arguments as a string Key Value
        /// </summary>
        public NameValueCollection Argument { get; } = new NameValueCollection();

        public int NumberOfTries { get; set; }
    }
}
