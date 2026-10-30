namespace AngleSharp.Js.Cache
{
    using Acornima.Ast;
    using Jint;
    using Jint.Runtime;
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Caches the parsed and analyzed form of scripts. A prepared script is
    /// documented by Jint as reusable and thread-safe, so the same one can serve
    /// every document that runs the same source with the same regex timeout - a
    /// page loading a library ends up parsing it once instead of once per document.
    /// </summary>
    static class ScriptCache
    {
        //  There is no bound on how many distinct scripts a process may see, so the
        //  cache does not grow without end. The scripts worth keeping are the ones
        //  seen first: libraries are referenced early and by many documents.
        private const Int32 Capacity = 32;

        private static readonly ConcurrentDictionary<ScriptKey, Prepared<Script>> _scripts =
            new ConcurrentDictionary<ScriptKey, Prepared<Script>>();

        /// <summary>
        /// Gets the prepared form of the given source. An invalid result means the
        /// source could not be prepared and should be handed to the engine as text,
        /// so that the engine reports the syntax error itself.
        /// </summary>
        public static Prepared<Script> GetOrCreate(String source, TimeSpan regexTimeout)
        {
            var key = new ScriptKey(source, regexTimeout);

            if (_scripts.TryGetValue(key, out var prepared))
            {
                return prepared;
            }

            try
            {
                prepared = Engine.PrepareScript(source, options: new ScriptPreparationOptions
                {
                    ParsingOptions = new ScriptParsingOptions { RegexTimeout = regexTimeout },
                });
            }
            catch (ScriptPreparationException)
            {
                return default;
            }

            if (_scripts.Count < Capacity)
            {
                _scripts.TryAdd(key, prepared);
            }

            return prepared;
        }

        private readonly struct ScriptKey : IEquatable<ScriptKey>
        {
            private readonly String _source;
            private readonly TimeSpan _regexTimeout;

            public ScriptKey(String source, TimeSpan regexTimeout)
            {
                _source = source ?? throw new ArgumentNullException(nameof(source));
                _regexTimeout = regexTimeout;
            }

            public Boolean Equals(ScriptKey other) =>
                String.Equals(_source, other._source, StringComparison.Ordinal) && _regexTimeout == other._regexTimeout;

            public override Boolean Equals(Object obj) => obj is ScriptKey other && Equals(other);

            public override Int32 GetHashCode() => (_source.GetHashCode() * 397) ^ _regexTimeout.GetHashCode();
        }
    }
}
