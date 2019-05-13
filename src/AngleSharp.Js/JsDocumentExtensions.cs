namespace AngleSharp.Dom
{
    using AngleSharp.Browser;
    using AngleSharp.Js;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A set of extensions for interacting with the document.
    /// </summary>
    public static class JsDocumentExtensions
    {
        /// <summary>
        /// Enqueues the given action as a normal task to the document,
        /// thus ensuring it runs after any script-related actions.
        /// </summary>
        /// <param name="document">The document to extend.</param>
        /// <param name="action">The action on the document to perform.</param>
        /// <returns>A task that is finished when the enqueued task completed.</returns>
        public static Task<IDocument> Then(this IDocument document, Action<IDocument> action)
        {
            var context = document.Context;
            var evts = context.GetService<IEventLoop>();
            var tcs = new TaskCompletionSource<Boolean>();
            evts.Enqueue(cancel =>
            {
                try
                {
                    action?.Invoke(document);
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, TaskPriority.Normal);
            return tcs.Task.ContinueWith(_ => document);
        }

        /// <summary>
        /// Enqueues the given JavaScript code as a normal task to the document,
        /// thus ensuring it runs after any script-related actions.
        /// </summary>
        /// <param name="document">The document to extend.</param>
        /// <param name="jsSource">The JavaScript action to perform.</param>
        /// <returns>A task that is finished when the enqueued task completed.</returns>
        public static Task<IDocument> Then(this IDocument document, String jsSource) =>
            document.Then(current => current.ExecuteScript(jsSource));

        /// <summary>
        /// Enqueues the given action as a normal task to the document,
        /// thus ensuring it runs after any script-related actions.
        /// </summary>
        /// <param name="documentTask">The soon available document.</param>
        /// <param name="action">The action on the document to perform.</param>
        /// <returns>A task that is finished when the enqueued task completed.</returns>
        public static async Task<IDocument> Then(this Task<IDocument> documentTask, Action<IDocument> action)
        {
            var document = await documentTask.ConfigureAwait(false);
            return await document.Then(action).ConfigureAwait(false);
        }

        /// <summary>
        /// Enqueues the given JavaScript code as a normal task to the document,
        /// thus ensuring it runs after any script-related actions.
        /// </summary>
        /// <param name="documentTask">The soon available document.</param>
        /// <param name="jsSource">The JavaScript action to perform.</param>
        /// <returns>A task that is finished when the enqueued task completed.</returns>
        public static Task<IDocument> Then(this Task<IDocument> documentTask, String jsSource) =>
            documentTask.Then(document => document.ExecuteScript(jsSource));
    }
}
