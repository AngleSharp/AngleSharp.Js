namespace AngleSharp.Scripting.CSharp
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Css;
    using AngleSharp.Dom.Events;
    using AngleSharp.Dom.Navigator;
    using System;

    public abstract class WindowContext : IWindow
    {
        #region Fields

        readonly IWindow _window;

        #endregion

        #region ctor

        public WindowContext(IWindow window)
        {
            _window = window;
        }

        #endregion

        #region Properties

        public IDocument Document
        {
            get { return _window.Document; }
        }

        public IHistory History
        {
            get { return _window.History; }
        }

        public Boolean IsClosed
        {
            get { return _window.IsClosed; }
        }

        public ILocation Location
        {
            get { return _window.Location; }
        }

        public String Name
        {
            get { return _window.Name; }
            set { _window.Name = value; }
        }

        public INavigator Navigator
        {
            get { return _window.Navigator; }
        }

        public Int32 OuterHeight
        {
            get { return _window.OuterHeight; }
        }

        public Int32 OuterWidth
        {
            get { return _window.OuterWidth; }
        }

        public IWindow Proxy
        {
            get { return _window.Proxy; }
        }

        public Int32 ScreenX
        {
            get { return _window.ScreenX; }
        }

        public Int32 ScreenY
        {
            get { return _window.ScreenY; }
        }

        public String Status
        {
            get { return _window.Status; }
            set { _window.Status = value; }
        }

        #endregion

        #region Methods

        public IMediaQueryList MatchMedia(String media)
        {
            return _window.MatchMedia(media);
        }

        public void Alert(String message)
        {
            _window.Alert(message);
        }

        public void Blur()
        {
            _window.Blur();
        }

        public void Close()
        {
            _window.Close();
        }

        public Boolean Confirm(String message)
        {
            return _window.Confirm(message);
        }

        public void Focus()
        {
            _window.Focus();
        }

        public ICssStyleDeclaration GetComputedStyle(IElement element, String pseudo = null)
        {
            return _window.GetComputedStyle(element, pseudo);
        }

        public IWindow Open(String url = "about:blank", String name = null, String features = null, String replace = null)
        {
            return _window.Open(url, name, features, replace);
        }

        public void Print()
        {
            _window.Print();
        }

        public void Stop()
        {
            _window.Stop();
        }

        public void AddEventListener(String type, DomEventHandler callback = null, Boolean capture = false)
        {
            _window.AddEventListener(type, callback, capture);
        }

        public Boolean Dispatch(Event ev)
        {
            return _window.Dispatch(ev);
        }

        public void RemoveEventListener(String type, DomEventHandler callback = null, Boolean capture = false)
        {
            _window.RemoveEventListener(type, callback, capture);
        }

        public void InvokeEventListener(Event ev)
        {
            _window.InvokeEventListener(ev);
        }

        public void ClearInterval(Int32 handle = 0)
        {
            _window.ClearInterval(handle);
        }

        public void ClearTimeout(Int32 handle = 0)
        {
            _window.ClearTimeout(handle);
        }

        public Int32 SetInterval(Action<IWindow> handler, Int32 timeout = 0)
        {
            return _window.SetInterval(handler, timeout);
        }

        public Int32 SetTimeout(Action<IWindow> handler, Int32 timeout = 0)
        {
            return _window.SetTimeout(handler, timeout);
        }

        #endregion

        #region Events

        public event DomEventHandler Aborted
        {
            add { _window.Aborted += value; }
            remove { _window.Aborted -= value; }
        }

        public event DomEventHandler Blurred
        {
            add { _window.Blurred += value; }
            remove { _window.Blurred -= value; }
        }

        public event DomEventHandler CanPlay
        {
            add { _window.CanPlay += value; }
            remove { _window.CanPlay -= value; }
        }

        public event DomEventHandler CanPlayThrough
        {
            add { _window.CanPlayThrough += value; }
            remove { _window.CanPlayThrough -= value; }
        }

        public event DomEventHandler Cancelled
        {
            add { _window.Cancelled += value; }
            remove { _window.Cancelled -= value; }
        }

        public event DomEventHandler Changed
        {
            add { _window.Changed += value; }
            remove { _window.Changed -= value; }
        }

        public event DomEventHandler Clicked
        {
            add { _window.Clicked += value; }
            remove { _window.Clicked -= value; }
        }

        public event DomEventHandler CueChanged
        {
            add { _window.CueChanged += value; }
            remove { _window.CueChanged -= value; }
        }

        public event DomEventHandler DoubleClick
        {
            add { _window.DoubleClick += value; }
            remove { _window.DoubleClick -= value; }
        }

        public event DomEventHandler Drag
        {
            add { _window.Drag += value; }
            remove { _window.Drag -= value; }
        }

        public event DomEventHandler DragEnd
        {
            add { _window.DragEnd += value; }
            remove { _window.DragEnd -= value; }
        }

        public event DomEventHandler DragEnter
        {
            add { _window.DragEnter += value; }
            remove { _window.DragEnter -= value; }
        }

        public event DomEventHandler DragExit
        {
            add { _window.DragExit += value; }
            remove { _window.DragExit -= value; }
        }

        public event DomEventHandler DragLeave
        {
            add { _window.DragLeave += value; }
            remove { _window.DragLeave -= value; }
        }

        public event DomEventHandler DragOver
        {
            add { _window.DragOver += value; }
            remove { _window.DragOver -= value; }
        }

        public event DomEventHandler DragStart
        {
            add { _window.DragStart += value; }
            remove { _window.DragStart -= value; }
        }

        public event DomEventHandler Dropped
        {
            add { _window.Dropped += value; }
            remove { _window.Dropped -= value; }
        }

        public event DomEventHandler DurationChanged
        {
            add { _window.DurationChanged += value; }
            remove { _window.DurationChanged -= value; }
        }

        public event DomEventHandler Emptied
        {
            add { _window.Emptied += value; }
            remove { _window.Emptied -= value; }
        }

        public event DomEventHandler Ended
        {
            add { _window.Ended += value; }
            remove { _window.Ended -= value; }
        }

        public event DomEventHandler Error
        {
            add { _window.Error += value; }
            remove { _window.Error -= value; }
        }

        public event DomEventHandler Focused
        {
            add { _window.Focused += value; }
            remove { _window.Focused -= value; }
        }

        public event DomEventHandler Input
        {
            add { _window.Input += value; }
            remove { _window.Input -= value; }
        }

        public event DomEventHandler Invalid
        {
            add { _window.Invalid += value; }
            remove { _window.Invalid -= value; }
        }

        public event DomEventHandler KeyDown
        {
            add { _window.KeyDown += value; }
            remove { _window.KeyDown -= value; }
        }

        public event DomEventHandler KeyPress
        {
            add { _window.KeyPress += value; }
            remove { _window.KeyPress -= value; }
        }

        public event DomEventHandler KeyUp
        {
            add { _window.KeyUp += value; }
            remove { _window.KeyUp -= value; }
        }

        public event DomEventHandler Loaded
        {
            add { _window.Loaded += value; }
            remove { _window.Loaded -= value; }
        }

        public event DomEventHandler LoadedData
        {
            add { _window.LoadedData += value; }
            remove { _window.LoadedData -= value; }
        }

        public event DomEventHandler LoadedMetadata
        {
            add { _window.LoadedMetadata += value; }
            remove { _window.LoadedMetadata -= value; }
        }

        public event DomEventHandler Loading
        {
            add { _window.Loading += value; }
            remove { _window.Loading -= value; }
        }

        public event DomEventHandler MouseDown
        {
            add { _window.MouseDown += value; }
            remove { _window.MouseDown -= value; }
        }

        public event DomEventHandler MouseEnter
        {
            add { _window.MouseEnter += value; }
            remove { _window.MouseEnter -= value; }
        }

        public event DomEventHandler MouseLeave
        {
            add { _window.MouseLeave += value; }
            remove { _window.MouseLeave -= value; }
        }

        public event DomEventHandler MouseMove
        {
            add { _window.MouseMove += value; }
            remove { _window.MouseMove -= value; }
        }

        public event DomEventHandler MouseOut
        {
            add { _window.MouseOut += value; }
            remove { _window.MouseOut -= value; }
        }

        public event DomEventHandler MouseOver
        {
            add { _window.MouseOver += value; }
            remove { _window.MouseOver -= value; }
        }

        public event DomEventHandler MouseUp
        {
            add { _window.MouseUp += value; }
            remove { _window.MouseUp -= value; }
        }

        public event DomEventHandler MouseWheel
        {
            add { _window.MouseWheel += value; }
            remove { _window.MouseWheel -= value; }
        }

        public event DomEventHandler Paused
        {
            add { _window.Paused += value; }
            remove { _window.Paused -= value; }
        }

        public event DomEventHandler Played
        {
            add { _window.Played += value; }
            remove { _window.Played -= value; }
        }

        public event DomEventHandler Playing
        {
            add { _window.Playing += value; }
            remove { _window.Playing -= value; }
        }

        public event DomEventHandler Progress
        {
            add { _window.Progress += value; }
            remove { _window.Progress -= value; }
        }

        public event DomEventHandler RateChanged
        {
            add { _window.RateChanged += value; }
            remove { _window.RateChanged -= value; }
        }

        public event DomEventHandler Resetted
        {
            add { _window.Resetted += value; }
            remove { _window.Resetted -= value; }
        }

        public event DomEventHandler Resized
        {
            add { _window.Resized += value; }
            remove { _window.Resized -= value; }
        }

        public event DomEventHandler Scrolled
        {
            add { _window.Scrolled += value; }
            remove { _window.Scrolled -= value; }
        }

        public event DomEventHandler Seeked
        {
            add { _window.Seeked += value; }
            remove { _window.Seeked -= value; }
        }

        public event DomEventHandler Seeking
        {
            add { _window.Seeking += value; }
            remove { _window.Seeking -= value; }
        }

        public event DomEventHandler Selected
        {
            add { _window.Selected += value; }
            remove { _window.Selected -= value; }
        }

        public event DomEventHandler Shown
        {
            add { _window.Shown += value; }
            remove { _window.Shown -= value; }
        }

        public event DomEventHandler Stalled
        {
            add { _window.Stalled += value; }
            remove { _window.Stalled -= value; }
        }

        public event DomEventHandler Submitted
        {
            add { _window.Submitted += value; }
            remove { _window.Submitted -= value; }
        }

        public event DomEventHandler Suspended
        {
            add { _window.Suspended += value; }
            remove { _window.Suspended -= value; }
        }

        public event DomEventHandler TimeUpdated
        {
            add { _window.TimeUpdated += value; }
            remove { _window.TimeUpdated -= value; }
        }

        public event DomEventHandler Toggled
        {
            add { _window.Toggled += value; }
            remove { _window.Toggled -= value; }
        }

        public event DomEventHandler VolumeChanged
        {
            add { _window.VolumeChanged += value; }
            remove { _window.VolumeChanged -= value; }
        }

        public event DomEventHandler Waiting
        {
            add { _window.Waiting += value; }
            remove { _window.Waiting -= value; }
        }

        public event DomEventHandler HashChanged
        {
            add { _window.HashChanged += value; }
            remove { _window.HashChanged -= value; }
        }

        public event DomEventHandler MessageReceived
        {
            add { _window.MessageReceived += value; }
            remove { _window.MessageReceived -= value; }
        }

        public event DomEventHandler PageHidden
        {
            add { _window.PageHidden += value; }
            remove { _window.PageHidden -= value; }
        }

        public event DomEventHandler PageShown
        {
            add { _window.PageShown += value; }
            remove { _window.PageShown -= value; }
        }

        public event DomEventHandler PopState
        {
            add { _window.PopState += value; }
            remove { _window.PopState -= value; }
        }

        public event DomEventHandler Printed
        {
            add { _window.Printed += value; }
            remove { _window.Printed -= value; }
        }

        public event DomEventHandler Printing
        {
            add { _window.Printing += value; }
            remove { _window.Printing -= value; }
        }

        public event DomEventHandler Storage
        {
            add { _window.Storage += value; }
            remove { _window.Storage -= value; }
        }

        public event DomEventHandler Unloaded
        {
            add { _window.Unloaded += value; }
            remove { _window.Unloaded -= value; }
        }

        public event DomEventHandler Unloading
        {
            add { _window.Unloading += value; }
            remove { _window.Unloading -= value; }
        }

        public event DomEventHandler WentOffline
        {
            add { _window.WentOffline += value; }
            remove { _window.WentOffline -= value; }
        }

        public event DomEventHandler WentOnline
        {
            add { _window.WentOnline += value; }
            remove { _window.WentOnline -= value; }
        }

        #endregion

        #region Context

        public abstract void Run();

        #endregion
    }
}
