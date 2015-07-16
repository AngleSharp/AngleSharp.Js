namespace AngleSharp.Scripting.CSharp
{
    using AngleSharp.Dom;
    using AngleSharp.Services;
    using System;

    sealed class WindowService : IWindowService
    {
        readonly Func<IWindow, WindowContext> _creator;

        public WindowService(Func<IWindow, WindowContext> creator)
        {
            _creator = creator;
        }

        public IWindow Create(IDocument document)
        {
            var existing = document.DefaultView;
            var context = _creator(existing);
            context.Run();
            return context;
        }
    }
}
