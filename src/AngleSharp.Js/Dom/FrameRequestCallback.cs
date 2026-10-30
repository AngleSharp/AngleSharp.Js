namespace AngleSharp.Js.Dom
{
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// The callback of requestAnimationFrame, receiving the time of the frame.
    /// </summary>
    /// <param name="window">The window, which is the receiver of the call.</param>
    /// <param name="time">The time of the frame, as given by performance.now().</param>
    public delegate void FrameRequestCallback(IWindow window, Double time);
}
