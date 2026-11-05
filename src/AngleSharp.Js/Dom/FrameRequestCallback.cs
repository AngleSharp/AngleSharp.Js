namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// The callback of requestAnimationFrame, receiving the time of the frame.
    /// </summary>
    /// <param name="window">The window, which is the receiver of the call.</param>
    /// <param name="time">The time of the frame, as given by performance.now().</param>
    public delegate void FrameRequestCallback(IWindow window, Double time);

    /// <summary>
    /// The callback of requestIdleCallback, receiving how much time it has.
    /// </summary>
    /// <param name="window">The window, which is the receiver of the call.</param>
    /// <param name="deadline">The time it has.</param>
    public delegate void IdleRequestCallback(IWindow window, IdleDeadline deadline);

    /// <summary>
    /// Represents the time a callback of requestIdleCallback has. There is no knowing how busy the
    /// page is going to be, so it is what a browser gives when nothing is waiting.
    /// </summary>
    [DomName("IdleDeadline")]
    public sealed class IdleDeadline
    {
        //  The longest period a browser gives, so that a page stays responsive (requestIdleCallback 1.3).
        private const Double Budget = 50.0;

        private readonly Performance _performance;
        private readonly Double _start;

        internal IdleDeadline(Performance performance)
        {
            _performance = performance;
            _start = performance.Now();
        }

        /// <summary>
        /// Gets if the callback is only called because its timeout has passed, which it never is.
        /// </summary>
        [DomName("didTimeout")]
        public Boolean DidTimeout => false;

        /// <summary>
        /// Gets the milliseconds the callback has left.
        /// </summary>
        [DomName("timeRemaining")]
        public Double TimeRemaining() => Math.Max(0.0, Budget - (_performance.Now() - _start));
    }
}
