namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using System;
    using System.Linq;

    /// <summary>
    /// What script sees of a window of another origin (HTML 7.2.1.3.1): only the members listed
    /// here exist, so that whatever else a window has or gets stays out of reach.
    /// </summary>
    [DomName("CrossOriginWindow")]
    [DomNoInterfaceObject]
    public sealed class CrossOriginWindow
    {
        private readonly IWindow _target;
        private readonly IDocument _viewer;
        private readonly CrossOriginLocation _location;

        internal CrossOriginWindow(IWindow target, IDocument viewer)
        {
            _target = target;
            _viewer = viewer;
            _location = new CrossOriginLocation(target, viewer);
        }

        /// <summary>
        /// Gets the window itself, as seen from the other origin.
        /// </summary>
        [DomName("window")]
        [DomName("self")]
        [DomName("frames")]
        public IWindow Self => _target;

        /// <summary>
        /// Gets the window around it, which the engine turns into what the viewer may see of it.
        /// </summary>
        [DomName("parent")]
        public IWindow Parent => _target.Parent();

        /// <summary>
        /// Gets the top window, which the engine turns into what the viewer may see of it.
        /// </summary>
        [DomName("top")]
        public IWindow Top => _target.Top();

        /// <summary>
        /// Gets if the window is closed.
        /// </summary>
        [DomName("closed")]
        public Boolean IsClosed => _target.IsClosed;

        /// <summary>
        /// Gets the number of frames in the window.
        /// </summary>
        [DomName("length")]
        public Int32 Length => _target.Document.QuerySelectorAll("iframe").Length;

        /// <summary>
        /// Gets the location, of which only navigating is available.
        /// </summary>
        [DomName("location")]
        [DomPutForwards("href")]
        public CrossOriginLocation Location => _location;

        /// <summary>
        /// Posts a message to the window.
        /// </summary>
        [DomName("postMessage")]
        public void PostMessage(Object message, Object targetOrigin = null, Object transfer = null) =>
            WindowExtensions.PostMessageFrom(_viewer, _target, message, targetOrigin, transfer);

        /// <summary>
        /// Focuses the window.
        /// </summary>
        [DomName("focus")]
        public void Focus() => _target.Focus();

        /// <summary>
        /// Removes the focus from the window.
        /// </summary>
        [DomName("blur")]
        public void Blur() => _target.Blur();

        /// <summary>
        /// Does nothing: only the window itself and who opened it close it.
        /// </summary>
        [DomName("close")]
        public void Close()
        {
        }
    }

    /// <summary>
    /// What script sees of the location of a window of another origin: it can be set, not read.
    /// </summary>
    [DomName("CrossOriginLocation")]
    [DomNoInterfaceObject]
    public sealed class CrossOriginLocation
    {
        private readonly IWindow _target;
        private readonly IDocument _viewer;

        internal CrossOriginLocation(IWindow target, IDocument viewer)
        {
            _target = target;
            _viewer = viewer;
        }

        /// <summary>
        /// Navigates the window, which script may only do for frames inside its own document.
        /// Reading it throws, as the address of another origin is none of its business.
        /// </summary>
        [DomName("href")]
        public String Href
        {
            get => throw new DomException(DomError.Security);
            set => Navigate(value);
        }

        /// <summary>
        /// Navigates the window, see <see cref="Href"/>.
        /// </summary>
        [DomName("replace")]
        public void Replace(String url) => Navigate(url);

        /// <summary>
        /// Navigates the window, see <see cref="Href"/>.
        /// </summary>
        [DomName("assign")]
        public void Assign(String url) => Navigate(url);

        private void Navigate(String url)
        {
            // a frame taking over what is around it (or something next to it) is what framing
            // protections are about: browsers only let it happen after the user did something,
            // which isn't known here
            if (!IsInsideViewer())
            {
                throw new DomException(DomError.Security);
            }

            _target.Document.Location.Href = new Url(new Url(_viewer.BaseUri ?? String.Empty), url).Href;
        }

        private Boolean IsInsideViewer()
        {
            for (var context = _target.Document.Context?.Parent; context is not null; context = context.Parent)
            {
                if (ReferenceEquals(context, _viewer.Context))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
