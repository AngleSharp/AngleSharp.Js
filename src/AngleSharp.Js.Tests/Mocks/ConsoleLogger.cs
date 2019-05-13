namespace AngleSharp.Js.Tests.Mocks
{
    using System;
    using System.Text;

    sealed class ConsoleLogger : IConsoleLogger
    {
        private readonly StringBuilder _sb;

        public ConsoleLogger()
            : this(new StringBuilder())
        {
        }

        public ConsoleLogger(StringBuilder sb)
        {
            _sb = sb;
        }

        public StringBuilder Content
        {
            get { return _sb; }
        }

        public void Log(Object[] values)
        {
            foreach (var value in values)
            {
                _sb.AppendLine((value ?? "null").ToString());
            }
        }
    }
}
