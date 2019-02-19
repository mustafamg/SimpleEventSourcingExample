using System;

namespace Models
{
    [Serializable]
    public class Command<T>
    {
        public string Uri { get; set; }
        public HttpVerb Verb { get; set; }
        public T Argument { get; set; }
        public int NumberOfTries { get; set; }
    }
}
