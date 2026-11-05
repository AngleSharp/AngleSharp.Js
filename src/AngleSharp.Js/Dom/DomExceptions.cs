namespace AngleSharp.Js.Dom
{
    using Acornima.Ast;
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using System;

    /// <summary>
    /// The DOMException of scripts (WebIDL 3.14.1), which are errors as far as a script can tell:
    /// they have a stack and are an instance of Error. That is why it is a script, too.
    /// </summary>
    static class DomExceptions
    {
        //  The names by their legacy code; not every code has one anymore (WebIDL table 1).
        private static readonly String[] Names =
        {
            null,
            "IndexSizeError",
            "DOMStringSizeError",
            "HierarchyRequestError",
            "WrongDocumentError",
            "InvalidCharacterError",
            "NoDataAllowedError",
            "NoModificationAllowedError",
            "NotFoundError",
            "NotSupportedError",
            "InUseAttributeError",
            "InvalidStateError",
            "SyntaxError",
            "InvalidModificationError",
            "NamespaceError",
            "InvalidAccessError",
            "ValidationError",
            "TypeMismatchError",
            "SecurityError",
            "NetworkError",
            "AbortError",
            "URLMismatchError",
            "QuotaExceededError",
            "TimeoutError",
            "InvalidNodeTypeError",
            "DataCloneError",
        };

        private const String Source = @"(function () {
    var codes = {
        IndexSizeError: 1, HierarchyRequestError: 3, WrongDocumentError: 4, InvalidCharacterError: 5,
        NoModificationAllowedError: 7, NotFoundError: 8, NotSupportedError: 9, InUseAttributeError: 10,
        InvalidStateError: 11, SyntaxError: 12, InvalidModificationError: 13, NamespaceError: 14,
        InvalidAccessError: 15, TypeMismatchError: 17, SecurityError: 18, NetworkError: 19, AbortError: 20,
        URLMismatchError: 21, QuotaExceededError: 22, TimeoutError: 23, InvalidNodeTypeError: 24, DataCloneError: 25
    };
    var constants = {
        INDEX_SIZE_ERR: 1, DOMSTRING_SIZE_ERR: 2, HIERARCHY_REQUEST_ERR: 3, WRONG_DOCUMENT_ERR: 4,
        INVALID_CHARACTER_ERR: 5, NO_DATA_ALLOWED_ERR: 6, NO_MODIFICATION_ALLOWED_ERR: 7, NOT_FOUND_ERR: 8,
        NOT_SUPPORTED_ERR: 9, INUSE_ATTRIBUTE_ERR: 10, INVALID_STATE_ERR: 11, SYNTAX_ERR: 12,
        INVALID_MODIFICATION_ERR: 13, NAMESPACE_ERR: 14, INVALID_ACCESS_ERR: 15, VALIDATION_ERR: 16,
        TYPE_MISMATCH_ERR: 17, SECURITY_ERR: 18, NETWORK_ERR: 19, ABORT_ERR: 20, URL_MISMATCH_ERR: 21,
        QUOTA_EXCEEDED_ERR: 22, TIMEOUT_ERR: 23, INVALID_NODE_TYPE_ERR: 24, DATA_CLONE_ERR: 25
    };
    function DOMException(message, name) {
        if (!new.target) {
            throw new TypeError(""Constructor DOMException requires 'new'"");
        }
        var self = new Error(message === undefined ? '' : String(message));
        var given = name === undefined ? 'Error' : String(name);
        Object.setPrototypeOf(self, new.target.prototype);
        Object.defineProperty(self, 'name', { value: given, configurable: true });
        Object.defineProperty(self, 'code', { value: codes[given] || 0, configurable: true });
        return self;
    }
    DOMException.prototype = Object.create(Error.prototype, {
        constructor: { value: DOMException, writable: true, configurable: true }
    });
    Object.defineProperty(DOMException, 'prototype', { writable: false });
    Object.keys(constants).forEach(function (key) {
        var constant = { value: constants[key], enumerable: true };
        Object.defineProperty(DOMException, key, constant);
        Object.defineProperty(DOMException.prototype, key, constant);
    });
    return DOMException;
})()";

        //  Reusable and thread-safe, like the scripts of pages (see ScriptCache).
        private static readonly Lazy<Prepared<Script>> Prepared = new Lazy<Prepared<Script>>(() => Engine.PrepareScript(Source));

        /// <summary>
        /// Creates the constructor for the given engine, before any script of a page runs.
        /// </summary>
        public static JsValue CreateConstructor(Engine engine)
        {
            var prepared = Prepared.Value;
            return engine.Evaluate(prepared);
        }

        /// <summary>
        /// Creates what a script gets for the exception of the DOM. The ones made from just a message
        /// don't tell a code, and have the message as name.
        /// </summary>
        public static JsValue Create(Engine engine, JsValue constructor, DomException exception)
        {
            var code = exception.Code;
            var isKnown = code > 0 && code < Names.Length && Names[code] != null;
            var message = code == 0 ? exception.Name : exception.Message;

            //  the names added to the standard later (e.g. NotAllowedError) have no code
            var name = isKnown ? Names[code] : exception.Name.EndsWith("Error", StringComparison.Ordinal) ? exception.Name : "Error";
            return engine.Construct(constructor, new JsValue[] { message, name });
        }
    }
}
