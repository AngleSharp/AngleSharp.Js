namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class CSSStyleDeclarationPrototype : CSSStyleDeclarationInstance
    {
        readonly EngineInstance _engine;

        public CSSStyleDeclarationPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getPropertyValue", Engine.AsValue(GetPropertyValue), true, true, true);
            FastAddProperty("getPropertyPriority", Engine.AsValue(GetPropertyPriority), true, true, true);
            FastAddProperty("setProperty", Engine.AsValue(SetProperty), true, true, true);
            FastAddProperty("removeProperty", Engine.AsValue(RemoveProperty), true, true, true);
            FastSetProperty("cssText", Engine.AsProperty(GetCssText, SetCssText));
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("parentRule", Engine.AsProperty(GetParentRule));
            FastSetProperty("accelerator", Engine.AsProperty(GetAccelerator, SetAccelerator));
            FastSetProperty("alignContent", Engine.AsProperty(GetAlignContent, SetAlignContent));
            FastSetProperty("alignItems", Engine.AsProperty(GetAlignItems, SetAlignItems));
            FastSetProperty("alignmentBaseline", Engine.AsProperty(GetAlignmentBaseline, SetAlignmentBaseline));
            FastSetProperty("alignSelf", Engine.AsProperty(GetAlignSelf, SetAlignSelf));
            FastSetProperty("animation", Engine.AsProperty(GetAnimation, SetAnimation));
            FastSetProperty("animationDelay", Engine.AsProperty(GetAnimationDelay, SetAnimationDelay));
            FastSetProperty("animationDirection", Engine.AsProperty(GetAnimationDirection, SetAnimationDirection));
            FastSetProperty("animationDuration", Engine.AsProperty(GetAnimationDuration, SetAnimationDuration));
            FastSetProperty("animationFillMode", Engine.AsProperty(GetAnimationFillMode, SetAnimationFillMode));
            FastSetProperty("animationIterationCount", Engine.AsProperty(GetAnimationIterationCount, SetAnimationIterationCount));
            FastSetProperty("animationName", Engine.AsProperty(GetAnimationName, SetAnimationName));
            FastSetProperty("animationPlayState", Engine.AsProperty(GetAnimationPlayState, SetAnimationPlayState));
            FastSetProperty("animationTimingFunction", Engine.AsProperty(GetAnimationTimingFunction, SetAnimationTimingFunction));
            FastSetProperty("backfaceVisibility", Engine.AsProperty(GetBackfaceVisibility, SetBackfaceVisibility));
            FastSetProperty("background", Engine.AsProperty(GetBackground, SetBackground));
            FastSetProperty("backgroundAttachment", Engine.AsProperty(GetBackgroundAttachment, SetBackgroundAttachment));
            FastSetProperty("backgroundClip", Engine.AsProperty(GetBackgroundClip, SetBackgroundClip));
            FastSetProperty("backgroundColor", Engine.AsProperty(GetBackgroundColor, SetBackgroundColor));
            FastSetProperty("backgroundImage", Engine.AsProperty(GetBackgroundImage, SetBackgroundImage));
            FastSetProperty("backgroundOrigin", Engine.AsProperty(GetBackgroundOrigin, SetBackgroundOrigin));
            FastSetProperty("backgroundPosition", Engine.AsProperty(GetBackgroundPosition, SetBackgroundPosition));
            FastSetProperty("backgroundPositionX", Engine.AsProperty(GetBackgroundPositionX, SetBackgroundPositionX));
            FastSetProperty("backgroundPositionY", Engine.AsProperty(GetBackgroundPositionY, SetBackgroundPositionY));
            FastSetProperty("backgroundRepeat", Engine.AsProperty(GetBackgroundRepeat, SetBackgroundRepeat));
            FastSetProperty("backgroundSize", Engine.AsProperty(GetBackgroundSize, SetBackgroundSize));
            FastSetProperty("baselineShift", Engine.AsProperty(GetBaselineShift, SetBaselineShift));
            FastSetProperty("behavior", Engine.AsProperty(GetBehavior, SetBehavior));
            FastSetProperty("border", Engine.AsProperty(GetBorder, SetBorder));
            FastSetProperty("borderBottom", Engine.AsProperty(GetBorderBottom, SetBorderBottom));
            FastSetProperty("borderBottomColor", Engine.AsProperty(GetBorderBottomColor, SetBorderBottomColor));
            FastSetProperty("borderBottomLeftRadius", Engine.AsProperty(GetBorderBottomLeftRadius, SetBorderBottomLeftRadius));
            FastSetProperty("borderBottomRightRadius", Engine.AsProperty(GetBorderBottomRightRadius, SetBorderBottomRightRadius));
            FastSetProperty("borderBottomStyle", Engine.AsProperty(GetBorderBottomStyle, SetBorderBottomStyle));
            FastSetProperty("borderBottomWidth", Engine.AsProperty(GetBorderBottomWidth, SetBorderBottomWidth));
            FastSetProperty("borderCollapse", Engine.AsProperty(GetBorderCollapse, SetBorderCollapse));
            FastSetProperty("borderColor", Engine.AsProperty(GetBorderColor, SetBorderColor));
            FastSetProperty("borderImage", Engine.AsProperty(GetBorderImage, SetBorderImage));
            FastSetProperty("borderImageOutset", Engine.AsProperty(GetBorderImageOutset, SetBorderImageOutset));
            FastSetProperty("borderImageRepeat", Engine.AsProperty(GetBorderImageRepeat, SetBorderImageRepeat));
            FastSetProperty("borderImageSlice", Engine.AsProperty(GetBorderImageSlice, SetBorderImageSlice));
            FastSetProperty("borderImageSource", Engine.AsProperty(GetBorderImageSource, SetBorderImageSource));
            FastSetProperty("borderImageWidth", Engine.AsProperty(GetBorderImageWidth, SetBorderImageWidth));
            FastSetProperty("borderLeft", Engine.AsProperty(GetBorderLeft, SetBorderLeft));
            FastSetProperty("borderLeftColor", Engine.AsProperty(GetBorderLeftColor, SetBorderLeftColor));
            FastSetProperty("borderLeftStyle", Engine.AsProperty(GetBorderLeftStyle, SetBorderLeftStyle));
            FastSetProperty("borderLeftWidth", Engine.AsProperty(GetBorderLeftWidth, SetBorderLeftWidth));
            FastSetProperty("borderRadius", Engine.AsProperty(GetBorderRadius, SetBorderRadius));
            FastSetProperty("borderRight", Engine.AsProperty(GetBorderRight, SetBorderRight));
            FastSetProperty("borderRightColor", Engine.AsProperty(GetBorderRightColor, SetBorderRightColor));
            FastSetProperty("borderRightStyle", Engine.AsProperty(GetBorderRightStyle, SetBorderRightStyle));
            FastSetProperty("borderRightWidth", Engine.AsProperty(GetBorderRightWidth, SetBorderRightWidth));
            FastSetProperty("borderSpacing", Engine.AsProperty(GetBorderSpacing, SetBorderSpacing));
            FastSetProperty("borderStyle", Engine.AsProperty(GetBorderStyle, SetBorderStyle));
            FastSetProperty("borderTop", Engine.AsProperty(GetBorderTop, SetBorderTop));
            FastSetProperty("borderTopColor", Engine.AsProperty(GetBorderTopColor, SetBorderTopColor));
            FastSetProperty("borderTopLeftRadius", Engine.AsProperty(GetBorderTopLeftRadius, SetBorderTopLeftRadius));
            FastSetProperty("borderTopRightRadius", Engine.AsProperty(GetBorderTopRightRadius, SetBorderTopRightRadius));
            FastSetProperty("borderTopStyle", Engine.AsProperty(GetBorderTopStyle, SetBorderTopStyle));
            FastSetProperty("borderTopWidth", Engine.AsProperty(GetBorderTopWidth, SetBorderTopWidth));
            FastSetProperty("borderWidth", Engine.AsProperty(GetBorderWidth, SetBorderWidth));
            FastSetProperty("boxShadow", Engine.AsProperty(GetBoxShadow, SetBoxShadow));
            FastSetProperty("boxSizing", Engine.AsProperty(GetBoxSizing, SetBoxSizing));
            FastSetProperty("breakAfter", Engine.AsProperty(GetBreakAfter, SetBreakAfter));
            FastSetProperty("breakBefore", Engine.AsProperty(GetBreakBefore, SetBreakBefore));
            FastSetProperty("breakInside", Engine.AsProperty(GetBreakInside, SetBreakInside));
            FastSetProperty("captionSide", Engine.AsProperty(GetCaptionSide, SetCaptionSide));
            FastSetProperty("clear", Engine.AsProperty(GetClear, SetClear));
            FastSetProperty("clip", Engine.AsProperty(GetClip, SetClip));
            FastSetProperty("clipBottom", Engine.AsProperty(GetClipBottom, SetClipBottom));
            FastSetProperty("clipLeft", Engine.AsProperty(GetClipLeft, SetClipLeft));
            FastSetProperty("clipPath", Engine.AsProperty(GetClipPath, SetClipPath));
            FastSetProperty("clipRight", Engine.AsProperty(GetClipRight, SetClipRight));
            FastSetProperty("clipRule", Engine.AsProperty(GetClipRule, SetClipRule));
            FastSetProperty("clipTop", Engine.AsProperty(GetClipTop, SetClipTop));
            FastSetProperty("color", Engine.AsProperty(GetColor, SetColor));
            FastSetProperty("colorInterpolationFilters", Engine.AsProperty(GetColorInterpolationFilters, SetColorInterpolationFilters));
            FastSetProperty("columnCount", Engine.AsProperty(GetColumnCount, SetColumnCount));
            FastSetProperty("columnFill", Engine.AsProperty(GetColumnFill, SetColumnFill));
            FastSetProperty("columnGap", Engine.AsProperty(GetColumnGap, SetColumnGap));
            FastSetProperty("columnRule", Engine.AsProperty(GetColumnRule, SetColumnRule));
            FastSetProperty("columnRuleColor", Engine.AsProperty(GetColumnRuleColor, SetColumnRuleColor));
            FastSetProperty("columnRuleStyle", Engine.AsProperty(GetColumnRuleStyle, SetColumnRuleStyle));
            FastSetProperty("columnRuleWidth", Engine.AsProperty(GetColumnRuleWidth, SetColumnRuleWidth));
            FastSetProperty("columns", Engine.AsProperty(GetColumns, SetColumns));
            FastSetProperty("columnSpan", Engine.AsProperty(GetColumnSpan, SetColumnSpan));
            FastSetProperty("columnWidth", Engine.AsProperty(GetColumnWidth, SetColumnWidth));
            FastSetProperty("content", Engine.AsProperty(GetContent, SetContent));
            FastSetProperty("counterIncrement", Engine.AsProperty(GetCounterIncrement, SetCounterIncrement));
            FastSetProperty("counterReset", Engine.AsProperty(GetCounterReset, SetCounterReset));
            FastSetProperty("cursor", Engine.AsProperty(GetCursor, SetCursor));
            FastSetProperty("direction", Engine.AsProperty(GetDirection, SetDirection));
            FastSetProperty("display", Engine.AsProperty(GetDisplay, SetDisplay));
            FastSetProperty("dominantBaseline", Engine.AsProperty(GetDominantBaseline, SetDominantBaseline));
            FastSetProperty("emptyCells", Engine.AsProperty(GetEmptyCells, SetEmptyCells));
            FastSetProperty("enableBackground", Engine.AsProperty(GetEnableBackground, SetEnableBackground));
            FastSetProperty("fill", Engine.AsProperty(GetFill, SetFill));
            FastSetProperty("fillOpacity", Engine.AsProperty(GetFillOpacity, SetFillOpacity));
            FastSetProperty("fillRule", Engine.AsProperty(GetFillRule, SetFillRule));
            FastSetProperty("filter", Engine.AsProperty(GetFilter, SetFilter));
            FastSetProperty("flex", Engine.AsProperty(GetFlex, SetFlex));
            FastSetProperty("flexBasis", Engine.AsProperty(GetFlexBasis, SetFlexBasis));
            FastSetProperty("flexDirection", Engine.AsProperty(GetFlexDirection, SetFlexDirection));
            FastSetProperty("flexFlow", Engine.AsProperty(GetFlexFlow, SetFlexFlow));
            FastSetProperty("flexGrow", Engine.AsProperty(GetFlexGrow, SetFlexGrow));
            FastSetProperty("flexShrink", Engine.AsProperty(GetFlexShrink, SetFlexShrink));
            FastSetProperty("flexWrap", Engine.AsProperty(GetFlexWrap, SetFlexWrap));
            FastSetProperty("cssFloat", Engine.AsProperty(GetCssFloat, SetCssFloat));
            FastSetProperty("font", Engine.AsProperty(GetFont, SetFont));
            FastSetProperty("fontFamily", Engine.AsProperty(GetFontFamily, SetFontFamily));
            FastSetProperty("fontFeatureSettings", Engine.AsProperty(GetFontFeatureSettings, SetFontFeatureSettings));
            FastSetProperty("fontSize", Engine.AsProperty(GetFontSize, SetFontSize));
            FastSetProperty("fontSizeAdjust", Engine.AsProperty(GetFontSizeAdjust, SetFontSizeAdjust));
            FastSetProperty("fontStretch", Engine.AsProperty(GetFontStretch, SetFontStretch));
            FastSetProperty("fontStyle", Engine.AsProperty(GetFontStyle, SetFontStyle));
            FastSetProperty("fontVariant", Engine.AsProperty(GetFontVariant, SetFontVariant));
            FastSetProperty("fontWeight", Engine.AsProperty(GetFontWeight, SetFontWeight));
            FastSetProperty("glyphOrientationHorizontal", Engine.AsProperty(GetGlyphOrientationHorizontal, SetGlyphOrientationHorizontal));
            FastSetProperty("glyphOrientationVertical", Engine.AsProperty(GetGlyphOrientationVertical, SetGlyphOrientationVertical));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("imeMode", Engine.AsProperty(GetImeMode, SetImeMode));
            FastSetProperty("justifyContent", Engine.AsProperty(GetJustifyContent, SetJustifyContent));
            FastSetProperty("layoutGrid", Engine.AsProperty(GetLayoutGrid, SetLayoutGrid));
            FastSetProperty("layoutGridChar", Engine.AsProperty(GetLayoutGridChar, SetLayoutGridChar));
            FastSetProperty("layoutGridLine", Engine.AsProperty(GetLayoutGridLine, SetLayoutGridLine));
            FastSetProperty("layoutGridMode", Engine.AsProperty(GetLayoutGridMode, SetLayoutGridMode));
            FastSetProperty("layoutGridType", Engine.AsProperty(GetLayoutGridType, SetLayoutGridType));
            FastSetProperty("left", Engine.AsProperty(GetLeft, SetLeft));
            FastSetProperty("letterSpacing", Engine.AsProperty(GetLetterSpacing, SetLetterSpacing));
            FastSetProperty("lineHeight", Engine.AsProperty(GetLineHeight, SetLineHeight));
            FastSetProperty("listStyle", Engine.AsProperty(GetListStyle, SetListStyle));
            FastSetProperty("listStyleImage", Engine.AsProperty(GetListStyleImage, SetListStyleImage));
            FastSetProperty("listStylePosition", Engine.AsProperty(GetListStylePosition, SetListStylePosition));
            FastSetProperty("listStyleType", Engine.AsProperty(GetListStyleType, SetListStyleType));
            FastSetProperty("margin", Engine.AsProperty(GetMargin, SetMargin));
            FastSetProperty("marginBottom", Engine.AsProperty(GetMarginBottom, SetMarginBottom));
            FastSetProperty("marginLeft", Engine.AsProperty(GetMarginLeft, SetMarginLeft));
            FastSetProperty("marginRight", Engine.AsProperty(GetMarginRight, SetMarginRight));
            FastSetProperty("marginTop", Engine.AsProperty(GetMarginTop, SetMarginTop));
            FastSetProperty("marker", Engine.AsProperty(GetMarker, SetMarker));
            FastSetProperty("markerEnd", Engine.AsProperty(GetMarkerEnd, SetMarkerEnd));
            FastSetProperty("markerMid", Engine.AsProperty(GetMarkerMid, SetMarkerMid));
            FastSetProperty("markerStart", Engine.AsProperty(GetMarkerStart, SetMarkerStart));
            FastSetProperty("mask", Engine.AsProperty(GetMask, SetMask));
            FastSetProperty("maxHeight", Engine.AsProperty(GetMaxHeight, SetMaxHeight));
            FastSetProperty("maxWidth", Engine.AsProperty(GetMaxWidth, SetMaxWidth));
            FastSetProperty("minHeight", Engine.AsProperty(GetMinHeight, SetMinHeight));
            FastSetProperty("minWidth", Engine.AsProperty(GetMinWidth, SetMinWidth));
            FastSetProperty("opacity", Engine.AsProperty(GetOpacity, SetOpacity));
            FastSetProperty("order", Engine.AsProperty(GetOrder, SetOrder));
            FastSetProperty("orphans", Engine.AsProperty(GetOrphans, SetOrphans));
            FastSetProperty("outline", Engine.AsProperty(GetOutline, SetOutline));
            FastSetProperty("outlineColor", Engine.AsProperty(GetOutlineColor, SetOutlineColor));
            FastSetProperty("outlineStyle", Engine.AsProperty(GetOutlineStyle, SetOutlineStyle));
            FastSetProperty("outlineWidth", Engine.AsProperty(GetOutlineWidth, SetOutlineWidth));
            FastSetProperty("overflow", Engine.AsProperty(GetOverflow, SetOverflow));
            FastSetProperty("overflowX", Engine.AsProperty(GetOverflowX, SetOverflowX));
            FastSetProperty("overflowY", Engine.AsProperty(GetOverflowY, SetOverflowY));
            FastSetProperty("padding", Engine.AsProperty(GetPadding, SetPadding));
            FastSetProperty("paddingBottom", Engine.AsProperty(GetPaddingBottom, SetPaddingBottom));
            FastSetProperty("paddingLeft", Engine.AsProperty(GetPaddingLeft, SetPaddingLeft));
            FastSetProperty("paddingRight", Engine.AsProperty(GetPaddingRight, SetPaddingRight));
            FastSetProperty("paddingTop", Engine.AsProperty(GetPaddingTop, SetPaddingTop));
            FastSetProperty("pageBreakAfter", Engine.AsProperty(GetPageBreakAfter, SetPageBreakAfter));
            FastSetProperty("pageBreakBefore", Engine.AsProperty(GetPageBreakBefore, SetPageBreakBefore));
            FastSetProperty("pageBreakInside", Engine.AsProperty(GetPageBreakInside, SetPageBreakInside));
            FastSetProperty("perspective", Engine.AsProperty(GetPerspective, SetPerspective));
            FastSetProperty("perspectiveOrigin", Engine.AsProperty(GetPerspectiveOrigin, SetPerspectiveOrigin));
            FastSetProperty("pointerEvents", Engine.AsProperty(GetPointerEvents, SetPointerEvents));
            FastSetProperty("position", Engine.AsProperty(GetPosition, SetPosition));
            FastSetProperty("quotes", Engine.AsProperty(GetQuotes, SetQuotes));
            FastSetProperty("right", Engine.AsProperty(GetRight, SetRight));
            FastSetProperty("rubyAlign", Engine.AsProperty(GetRubyAlign, SetRubyAlign));
            FastSetProperty("rubyOverhang", Engine.AsProperty(GetRubyOverhang, SetRubyOverhang));
            FastSetProperty("rubyPosition", Engine.AsProperty(GetRubyPosition, SetRubyPosition));
            FastSetProperty("scrollbar3dLightColor", Engine.AsProperty(GetScrollbar3dLightColor, SetScrollbar3dLightColor));
            FastSetProperty("scrollbarArrowColor", Engine.AsProperty(GetScrollbarArrowColor, SetScrollbarArrowColor));
            FastSetProperty("scrollbarDarkShadowColor", Engine.AsProperty(GetScrollbarDarkShadowColor, SetScrollbarDarkShadowColor));
            FastSetProperty("scrollbarFaceColor", Engine.AsProperty(GetScrollbarFaceColor, SetScrollbarFaceColor));
            FastSetProperty("scrollbarHighlightColor", Engine.AsProperty(GetScrollbarHighlightColor, SetScrollbarHighlightColor));
            FastSetProperty("scrollbarShadowColor", Engine.AsProperty(GetScrollbarShadowColor, SetScrollbarShadowColor));
            FastSetProperty("scrollbarTrackColor", Engine.AsProperty(GetScrollbarTrackColor, SetScrollbarTrackColor));
            FastSetProperty("stroke", Engine.AsProperty(GetStroke, SetStroke));
            FastSetProperty("strokeDasharray", Engine.AsProperty(GetStrokeDasharray, SetStrokeDasharray));
            FastSetProperty("strokeDashoffset", Engine.AsProperty(GetStrokeDashoffset, SetStrokeDashoffset));
            FastSetProperty("strokeLinecap", Engine.AsProperty(GetStrokeLinecap, SetStrokeLinecap));
            FastSetProperty("strokeLinejoin", Engine.AsProperty(GetStrokeLinejoin, SetStrokeLinejoin));
            FastSetProperty("strokeMiterlimit", Engine.AsProperty(GetStrokeMiterlimit, SetStrokeMiterlimit));
            FastSetProperty("strokeOpacity", Engine.AsProperty(GetStrokeOpacity, SetStrokeOpacity));
            FastSetProperty("strokeWidth", Engine.AsProperty(GetStrokeWidth, SetStrokeWidth));
            FastSetProperty("tableLayout", Engine.AsProperty(GetTableLayout, SetTableLayout));
            FastSetProperty("textAlign", Engine.AsProperty(GetTextAlign, SetTextAlign));
            FastSetProperty("textAlignLast", Engine.AsProperty(GetTextAlignLast, SetTextAlignLast));
            FastSetProperty("textAnchor", Engine.AsProperty(GetTextAnchor, SetTextAnchor));
            FastSetProperty("textAutospace", Engine.AsProperty(GetTextAutospace, SetTextAutospace));
            FastSetProperty("textDecoration", Engine.AsProperty(GetTextDecoration, SetTextDecoration));
            FastSetProperty("textIndent", Engine.AsProperty(GetTextIndent, SetTextIndent));
            FastSetProperty("textJustify", Engine.AsProperty(GetTextJustify, SetTextJustify));
            FastSetProperty("textOverflow", Engine.AsProperty(GetTextOverflow, SetTextOverflow));
            FastSetProperty("textShadow", Engine.AsProperty(GetTextShadow, SetTextShadow));
            FastSetProperty("textTransform", Engine.AsProperty(GetTextTransform, SetTextTransform));
            FastSetProperty("textUnderlinePosition", Engine.AsProperty(GetTextUnderlinePosition, SetTextUnderlinePosition));
            FastSetProperty("top", Engine.AsProperty(GetTop, SetTop));
            FastSetProperty("transform", Engine.AsProperty(GetTransform, SetTransform));
            FastSetProperty("transformOrigin", Engine.AsProperty(GetTransformOrigin, SetTransformOrigin));
            FastSetProperty("transformStyle", Engine.AsProperty(GetTransformStyle, SetTransformStyle));
            FastSetProperty("transition", Engine.AsProperty(GetTransition, SetTransition));
            FastSetProperty("transitionDelay", Engine.AsProperty(GetTransitionDelay, SetTransitionDelay));
            FastSetProperty("transitionDuration", Engine.AsProperty(GetTransitionDuration, SetTransitionDuration));
            FastSetProperty("transitionProperty", Engine.AsProperty(GetTransitionProperty, SetTransitionProperty));
            FastSetProperty("transitionTimingFunction", Engine.AsProperty(GetTransitionTimingFunction, SetTransitionTimingFunction));
            FastSetProperty("unicodeBidi", Engine.AsProperty(GetUnicodeBidi, SetUnicodeBidi));
            FastSetProperty("verticalAlign", Engine.AsProperty(GetVerticalAlign, SetVerticalAlign));
            FastSetProperty("visibility", Engine.AsProperty(GetVisibility, SetVisibility));
            FastSetProperty("whiteSpace", Engine.AsProperty(GetWhiteSpace, SetWhiteSpace));
            FastSetProperty("widows", Engine.AsProperty(GetWidows, SetWidows));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("wordBreak", Engine.AsProperty(GetWordBreak, SetWordBreak));
            FastSetProperty("wordSpacing", Engine.AsProperty(GetWordSpacing, SetWordSpacing));
            FastSetProperty("wordWrap", Engine.AsProperty(GetWordWrap, SetWordWrap));
            FastSetProperty("writingMode", Engine.AsProperty(GetWritingMode, SetWritingMode));
            FastSetProperty("zIndex", Engine.AsProperty(GetZIndex, SetZIndex));
            FastSetProperty("zoom", Engine.AsProperty(GetZoom, SetZoom));
        }

        public static CSSStyleDeclarationPrototype CreatePrototypeObject(EngineInstance engine, CSSStyleDeclarationConstructor constructor)
        {
            var obj = new CSSStyleDeclarationPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetPropertyValue(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var propertyName = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.GetPropertyValue(propertyName));
        }

        JsValue GetPropertyPriority(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var propertyName = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.GetPropertyPriority(propertyName));
        }

        JsValue SetProperty(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var propertyName = TypeConverter.ToString(arguments.At(0));
            var propertyValue = TypeConverter.ToString(arguments.At(1));
            var priority = TypeConverter.ToString(arguments.At(2));
            reference.SetProperty(propertyName, propertyValue, priority);
            return JsValue.Undefined;
        }

        JsValue RemoveProperty(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var propertyName = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.RemoveProperty(propertyName));
        }

        JsValue GetCssText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.CssText);
        }

        void SetCssText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.CssText = value;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue GetParentRule(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Parent);
        }


        JsValue GetAccelerator(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Accelerator);
        }

        void SetAccelerator(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Accelerator = value;
        }

        JsValue GetAlignContent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AlignContent);
        }

        void SetAlignContent(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AlignContent = value;
        }

        JsValue GetAlignItems(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AlignItems);
        }

        void SetAlignItems(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AlignItems = value;
        }

        JsValue GetAlignmentBaseline(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AlignmentBaseline);
        }

        void SetAlignmentBaseline(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AlignmentBaseline = value;
        }

        JsValue GetAlignSelf(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AlignSelf);
        }

        void SetAlignSelf(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AlignSelf = value;
        }

        JsValue GetAnimation(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Animation);
        }

        void SetAnimation(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Animation = value;
        }

        JsValue GetAnimationDelay(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationDelay);
        }

        void SetAnimationDelay(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationDelay = value;
        }

        JsValue GetAnimationDirection(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationDirection);
        }

        void SetAnimationDirection(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationDirection = value;
        }

        JsValue GetAnimationDuration(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationDuration);
        }

        void SetAnimationDuration(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationDuration = value;
        }

        JsValue GetAnimationFillMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationFillMode);
        }

        void SetAnimationFillMode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationFillMode = value;
        }

        JsValue GetAnimationIterationCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationIterationCount);
        }

        void SetAnimationIterationCount(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationIterationCount = value;
        }

        JsValue GetAnimationName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationName);
        }

        void SetAnimationName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationName = value;
        }

        JsValue GetAnimationPlayState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationPlayState);
        }

        void SetAnimationPlayState(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationPlayState = value;
        }

        JsValue GetAnimationTimingFunction(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.AnimationTimingFunction);
        }

        void SetAnimationTimingFunction(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.AnimationTimingFunction = value;
        }

        JsValue GetBackfaceVisibility(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackfaceVisibility);
        }

        void SetBackfaceVisibility(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackfaceVisibility = value;
        }

        JsValue GetBackground(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Background);
        }

        void SetBackground(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Background = value;
        }

        JsValue GetBackgroundAttachment(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundAttachment);
        }

        void SetBackgroundAttachment(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundAttachment = value;
        }

        JsValue GetBackgroundClip(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundClip);
        }

        void SetBackgroundClip(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundClip = value;
        }

        JsValue GetBackgroundColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundColor);
        }

        void SetBackgroundColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundColor = value;
        }

        JsValue GetBackgroundImage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundImage);
        }

        void SetBackgroundImage(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundImage = value;
        }

        JsValue GetBackgroundOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundOrigin);
        }

        void SetBackgroundOrigin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundOrigin = value;
        }

        JsValue GetBackgroundPosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundPosition);
        }

        void SetBackgroundPosition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundPosition = value;
        }

        JsValue GetBackgroundPositionX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundPositionX);
        }

        void SetBackgroundPositionX(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundPositionX = value;
        }

        JsValue GetBackgroundPositionY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundPositionY);
        }

        void SetBackgroundPositionY(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundPositionY = value;
        }

        JsValue GetBackgroundRepeat(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundRepeat);
        }

        void SetBackgroundRepeat(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundRepeat = value;
        }

        JsValue GetBackgroundSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BackgroundSize);
        }

        void SetBackgroundSize(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BackgroundSize = value;
        }

        JsValue GetBaselineShift(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BaselineShift);
        }

        void SetBaselineShift(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BaselineShift = value;
        }

        JsValue GetBehavior(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Behavior);
        }

        void SetBehavior(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Behavior = value;
        }

        JsValue GetBorder(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Border);
        }

        void SetBorder(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Border = value;
        }

        JsValue GetBorderBottom(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderBottom);
        }

        void SetBorderBottom(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderBottom = value;
        }

        JsValue GetBorderBottomColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderBottomColor);
        }

        void SetBorderBottomColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderBottomColor = value;
        }

        JsValue GetBorderBottomLeftRadius(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderBottomLeftRadius);
        }

        void SetBorderBottomLeftRadius(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderBottomLeftRadius = value;
        }

        JsValue GetBorderBottomRightRadius(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderBottomRightRadius);
        }

        void SetBorderBottomRightRadius(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderBottomRightRadius = value;
        }

        JsValue GetBorderBottomStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderBottomStyle);
        }

        void SetBorderBottomStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderBottomStyle = value;
        }

        JsValue GetBorderBottomWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderBottomWidth);
        }

        void SetBorderBottomWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderBottomWidth = value;
        }

        JsValue GetBorderCollapse(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderCollapse);
        }

        void SetBorderCollapse(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderCollapse = value;
        }

        JsValue GetBorderColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderColor);
        }

        void SetBorderColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderColor = value;
        }

        JsValue GetBorderImage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderImage);
        }

        void SetBorderImage(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderImage = value;
        }

        JsValue GetBorderImageOutset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderImageOutset);
        }

        void SetBorderImageOutset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderImageOutset = value;
        }

        JsValue GetBorderImageRepeat(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderImageRepeat);
        }

        void SetBorderImageRepeat(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderImageRepeat = value;
        }

        JsValue GetBorderImageSlice(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderImageSlice);
        }

        void SetBorderImageSlice(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderImageSlice = value;
        }

        JsValue GetBorderImageSource(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderImageSource);
        }

        void SetBorderImageSource(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderImageSource = value;
        }

        JsValue GetBorderImageWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderImageWidth);
        }

        void SetBorderImageWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderImageWidth = value;
        }

        JsValue GetBorderLeft(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderLeft);
        }

        void SetBorderLeft(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderLeft = value;
        }

        JsValue GetBorderLeftColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderLeftColor);
        }

        void SetBorderLeftColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderLeftColor = value;
        }

        JsValue GetBorderLeftStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderLeftStyle);
        }

        void SetBorderLeftStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderLeftStyle = value;
        }

        JsValue GetBorderLeftWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderLeftWidth);
        }

        void SetBorderLeftWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderLeftWidth = value;
        }

        JsValue GetBorderRadius(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderRadius);
        }

        void SetBorderRadius(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderRadius = value;
        }

        JsValue GetBorderRight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderRight);
        }

        void SetBorderRight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderRight = value;
        }

        JsValue GetBorderRightColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderRightColor);
        }

        void SetBorderRightColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderRightColor = value;
        }

        JsValue GetBorderRightStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderRightStyle);
        }

        void SetBorderRightStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderRightStyle = value;
        }

        JsValue GetBorderRightWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderRightWidth);
        }

        void SetBorderRightWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderRightWidth = value;
        }

        JsValue GetBorderSpacing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderSpacing);
        }

        void SetBorderSpacing(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderSpacing = value;
        }

        JsValue GetBorderStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderStyle);
        }

        void SetBorderStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderStyle = value;
        }

        JsValue GetBorderTop(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderTop);
        }

        void SetBorderTop(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderTop = value;
        }

        JsValue GetBorderTopColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderTopColor);
        }

        void SetBorderTopColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderTopColor = value;
        }

        JsValue GetBorderTopLeftRadius(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderTopLeftRadius);
        }

        void SetBorderTopLeftRadius(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderTopLeftRadius = value;
        }

        JsValue GetBorderTopRightRadius(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderTopRightRadius);
        }

        void SetBorderTopRightRadius(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderTopRightRadius = value;
        }

        JsValue GetBorderTopStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderTopStyle);
        }

        void SetBorderTopStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderTopStyle = value;
        }

        JsValue GetBorderTopWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderTopWidth);
        }

        void SetBorderTopWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderTopWidth = value;
        }

        JsValue GetBorderWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BorderWidth);
        }

        void SetBorderWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BorderWidth = value;
        }

        JsValue GetBoxShadow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BoxShadow);
        }

        void SetBoxShadow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BoxShadow = value;
        }

        JsValue GetBoxSizing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BoxSizing);
        }

        void SetBoxSizing(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BoxSizing = value;
        }

        JsValue GetBreakAfter(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BreakAfter);
        }

        void SetBreakAfter(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BreakAfter = value;
        }

        JsValue GetBreakBefore(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BreakBefore);
        }

        void SetBreakBefore(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BreakBefore = value;
        }

        JsValue GetBreakInside(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.BreakInside);
        }

        void SetBreakInside(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.BreakInside = value;
        }

        JsValue GetCaptionSide(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.CaptionSide);
        }

        void SetCaptionSide(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.CaptionSide = value;
        }

        JsValue GetClear(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Clear);
        }

        void SetClear(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Clear = value;
        }

        JsValue GetClip(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Clip);
        }

        void SetClip(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Clip = value;
        }

        JsValue GetClipBottom(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ClipBottom);
        }

        void SetClipBottom(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ClipBottom = value;
        }

        JsValue GetClipLeft(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ClipLeft);
        }

        void SetClipLeft(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ClipLeft = value;
        }

        JsValue GetClipPath(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ClipPath);
        }

        void SetClipPath(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ClipPath = value;
        }

        JsValue GetClipRight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ClipRight);
        }

        void SetClipRight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ClipRight = value;
        }

        JsValue GetClipRule(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ClipRule);
        }

        void SetClipRule(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ClipRule = value;
        }

        JsValue GetClipTop(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ClipTop);
        }

        void SetClipTop(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ClipTop = value;
        }

        JsValue GetColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Color);
        }

        void SetColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Color = value;
        }

        JsValue GetColorInterpolationFilters(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColorInterpolationFilters);
        }

        void SetColorInterpolationFilters(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColorInterpolationFilters = value;
        }

        JsValue GetColumnCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnCount);
        }

        void SetColumnCount(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnCount = value;
        }

        JsValue GetColumnFill(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnFill);
        }

        void SetColumnFill(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnFill = value;
        }

        JsValue GetColumnGap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnGap);
        }

        void SetColumnGap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnGap = value;
        }

        JsValue GetColumnRule(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnRule);
        }

        void SetColumnRule(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnRule = value;
        }

        JsValue GetColumnRuleColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnRuleColor);
        }

        void SetColumnRuleColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnRuleColor = value;
        }

        JsValue GetColumnRuleStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnRuleStyle);
        }

        void SetColumnRuleStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnRuleStyle = value;
        }

        JsValue GetColumnRuleWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnRuleWidth);
        }

        void SetColumnRuleWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnRuleWidth = value;
        }

        JsValue GetColumns(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Columns);
        }

        void SetColumns(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Columns = value;
        }

        JsValue GetColumnSpan(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnSpan);
        }

        void SetColumnSpan(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnSpan = value;
        }

        JsValue GetColumnWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ColumnWidth);
        }

        void SetColumnWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ColumnWidth = value;
        }

        JsValue GetContent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Content);
        }

        void SetContent(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Content = value;
        }

        JsValue GetCounterIncrement(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.CounterIncrement);
        }

        void SetCounterIncrement(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.CounterIncrement = value;
        }

        JsValue GetCounterReset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.CounterReset);
        }

        void SetCounterReset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.CounterReset = value;
        }

        JsValue GetCursor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Cursor);
        }

        void SetCursor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Cursor = value;
        }

        JsValue GetDirection(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Direction);
        }

        void SetDirection(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Direction = value;
        }

        JsValue GetDisplay(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Display);
        }

        void SetDisplay(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Display = value;
        }

        JsValue GetDominantBaseline(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.DominantBaseline);
        }

        void SetDominantBaseline(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.DominantBaseline = value;
        }

        JsValue GetEmptyCells(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.EmptyCells);
        }

        void SetEmptyCells(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.EmptyCells = value;
        }

        JsValue GetEnableBackground(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.EnableBackground);
        }

        void SetEnableBackground(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.EnableBackground = value;
        }

        JsValue GetFill(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Fill);
        }

        void SetFill(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Fill = value;
        }

        JsValue GetFillOpacity(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FillOpacity);
        }

        void SetFillOpacity(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FillOpacity = value;
        }

        JsValue GetFillRule(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FillRule);
        }

        void SetFillRule(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FillRule = value;
        }

        JsValue GetFilter(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Filter);
        }

        void SetFilter(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Filter = value;
        }

        JsValue GetFlex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Flex);
        }

        void SetFlex(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Flex = value;
        }

        JsValue GetFlexBasis(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FlexBasis);
        }

        void SetFlexBasis(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FlexBasis = value;
        }

        JsValue GetFlexDirection(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FlexDirection);
        }

        void SetFlexDirection(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FlexDirection = value;
        }

        JsValue GetFlexFlow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FlexFlow);
        }

        void SetFlexFlow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FlexFlow = value;
        }

        JsValue GetFlexGrow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FlexGrow);
        }

        void SetFlexGrow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FlexGrow = value;
        }

        JsValue GetFlexShrink(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FlexShrink);
        }

        void SetFlexShrink(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FlexShrink = value;
        }

        JsValue GetFlexWrap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FlexWrap);
        }

        void SetFlexWrap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FlexWrap = value;
        }

        JsValue GetCssFloat(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Float);
        }

        void SetCssFloat(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Float = value;
        }

        JsValue GetFont(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Font);
        }

        void SetFont(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Font = value;
        }

        JsValue GetFontFamily(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontFamily);
        }

        void SetFontFamily(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontFamily = value;
        }

        JsValue GetFontFeatureSettings(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontFeatureSettings);
        }

        void SetFontFeatureSettings(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontFeatureSettings = value;
        }

        JsValue GetFontSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontSize);
        }

        void SetFontSize(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontSize = value;
        }

        JsValue GetFontSizeAdjust(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontSizeAdjust);
        }

        void SetFontSizeAdjust(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontSizeAdjust = value;
        }

        JsValue GetFontStretch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontStretch);
        }

        void SetFontStretch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontStretch = value;
        }

        JsValue GetFontStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontStyle);
        }

        void SetFontStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontStyle = value;
        }

        JsValue GetFontVariant(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontVariant);
        }

        void SetFontVariant(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontVariant = value;
        }

        JsValue GetFontWeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.FontWeight);
        }

        void SetFontWeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.FontWeight = value;
        }

        JsValue GetGlyphOrientationHorizontal(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.GlyphOrientationHorizontal);
        }

        void SetGlyphOrientationHorizontal(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.GlyphOrientationHorizontal = value;
        }

        JsValue GetGlyphOrientationVertical(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.GlyphOrientationVertical);
        }

        void SetGlyphOrientationVertical(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.GlyphOrientationVertical = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Height);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Height = value;
        }

        JsValue GetImeMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ImeMode);
        }

        void SetImeMode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ImeMode = value;
        }

        JsValue GetJustifyContent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.JustifyContent);
        }

        void SetJustifyContent(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.JustifyContent = value;
        }

        JsValue GetLayoutGrid(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LayoutGrid);
        }

        void SetLayoutGrid(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LayoutGrid = value;
        }

        JsValue GetLayoutGridChar(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LayoutGridChar);
        }

        void SetLayoutGridChar(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LayoutGridChar = value;
        }

        JsValue GetLayoutGridLine(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LayoutGridLine);
        }

        void SetLayoutGridLine(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LayoutGridLine = value;
        }

        JsValue GetLayoutGridMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LayoutGridMode);
        }

        void SetLayoutGridMode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LayoutGridMode = value;
        }

        JsValue GetLayoutGridType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LayoutGridType);
        }

        void SetLayoutGridType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LayoutGridType = value;
        }

        JsValue GetLeft(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Left);
        }

        void SetLeft(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Left = value;
        }

        JsValue GetLetterSpacing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LetterSpacing);
        }

        void SetLetterSpacing(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LetterSpacing = value;
        }

        JsValue GetLineHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.LineHeight);
        }

        void SetLineHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.LineHeight = value;
        }

        JsValue GetListStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ListStyle);
        }

        void SetListStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ListStyle = value;
        }

        JsValue GetListStyleImage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ListStyleImage);
        }

        void SetListStyleImage(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ListStyleImage = value;
        }

        JsValue GetListStylePosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ListStylePosition);
        }

        void SetListStylePosition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ListStylePosition = value;
        }

        JsValue GetListStyleType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ListStyleType);
        }

        void SetListStyleType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ListStyleType = value;
        }

        JsValue GetMargin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Margin);
        }

        void SetMargin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Margin = value;
        }

        JsValue GetMarginBottom(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarginBottom);
        }

        void SetMarginBottom(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarginBottom = value;
        }

        JsValue GetMarginLeft(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarginLeft);
        }

        void SetMarginLeft(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarginLeft = value;
        }

        JsValue GetMarginRight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarginRight);
        }

        void SetMarginRight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarginRight = value;
        }

        JsValue GetMarginTop(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarginTop);
        }

        void SetMarginTop(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarginTop = value;
        }

        JsValue GetMarker(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Marker);
        }

        void SetMarker(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Marker = value;
        }

        JsValue GetMarkerEnd(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarkerEnd);
        }

        void SetMarkerEnd(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarkerEnd = value;
        }

        JsValue GetMarkerMid(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarkerMid);
        }

        void SetMarkerMid(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarkerMid = value;
        }

        JsValue GetMarkerStart(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MarkerStart);
        }

        void SetMarkerStart(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MarkerStart = value;
        }

        JsValue GetMask(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Mask);
        }

        void SetMask(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Mask = value;
        }

        JsValue GetMaxHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MaxHeight);
        }

        void SetMaxHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MaxHeight = value;
        }

        JsValue GetMaxWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MaxWidth);
        }

        void SetMaxWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MaxWidth = value;
        }

        JsValue GetMinHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MinHeight);
        }

        void SetMinHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MinHeight = value;
        }

        JsValue GetMinWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.MinWidth);
        }

        void SetMinWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.MinWidth = value;
        }

        JsValue GetOpacity(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Opacity);
        }

        void SetOpacity(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Opacity = value;
        }

        JsValue GetOrder(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Order);
        }

        void SetOrder(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Order = value;
        }

        JsValue GetOrphans(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Orphans);
        }

        void SetOrphans(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Orphans = value;
        }

        JsValue GetOutline(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Outline);
        }

        void SetOutline(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Outline = value;
        }

        JsValue GetOutlineColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.OutlineColor);
        }

        void SetOutlineColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.OutlineColor = value;
        }

        JsValue GetOutlineStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.OutlineStyle);
        }

        void SetOutlineStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.OutlineStyle = value;
        }

        JsValue GetOutlineWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.OutlineWidth);
        }

        void SetOutlineWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.OutlineWidth = value;
        }

        JsValue GetOverflow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Overflow);
        }

        void SetOverflow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Overflow = value;
        }

        JsValue GetOverflowX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.OverflowX);
        }

        void SetOverflowX(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.OverflowX = value;
        }

        JsValue GetOverflowY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.OverflowY);
        }

        void SetOverflowY(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.OverflowY = value;
        }

        JsValue GetPadding(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Padding);
        }

        void SetPadding(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Padding = value;
        }

        JsValue GetPaddingBottom(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PaddingBottom);
        }

        void SetPaddingBottom(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PaddingBottom = value;
        }

        JsValue GetPaddingLeft(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PaddingLeft);
        }

        void SetPaddingLeft(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PaddingLeft = value;
        }

        JsValue GetPaddingRight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PaddingRight);
        }

        void SetPaddingRight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PaddingRight = value;
        }

        JsValue GetPaddingTop(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PaddingTop);
        }

        void SetPaddingTop(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PaddingTop = value;
        }

        JsValue GetPageBreakAfter(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PageBreakAfter);
        }

        void SetPageBreakAfter(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PageBreakAfter = value;
        }

        JsValue GetPageBreakBefore(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PageBreakBefore);
        }

        void SetPageBreakBefore(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PageBreakBefore = value;
        }

        JsValue GetPageBreakInside(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PageBreakInside);
        }

        void SetPageBreakInside(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PageBreakInside = value;
        }

        JsValue GetPerspective(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Perspective);
        }

        void SetPerspective(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Perspective = value;
        }

        JsValue GetPerspectiveOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PerspectiveOrigin);
        }

        void SetPerspectiveOrigin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PerspectiveOrigin = value;
        }

        JsValue GetPointerEvents(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.PointerEvents);
        }

        void SetPointerEvents(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.PointerEvents = value;
        }

        JsValue GetPosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Position);
        }

        void SetPosition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Position = value;
        }

        JsValue GetQuotes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Quotes);
        }

        void SetQuotes(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Quotes = value;
        }

        JsValue GetRight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Right);
        }

        void SetRight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Right = value;
        }

        JsValue GetRubyAlign(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.RubyAlign);
        }

        void SetRubyAlign(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.RubyAlign = value;
        }

        JsValue GetRubyOverhang(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.RubyOverhang);
        }

        void SetRubyOverhang(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.RubyOverhang = value;
        }

        JsValue GetRubyPosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.RubyPosition);
        }

        void SetRubyPosition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.RubyPosition = value;
        }

        JsValue GetScrollbar3dLightColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Scrollbar3dLightColor);
        }

        void SetScrollbar3dLightColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Scrollbar3dLightColor = value;
        }

        JsValue GetScrollbarArrowColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ScrollbarArrowColor);
        }

        void SetScrollbarArrowColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ScrollbarArrowColor = value;
        }

        JsValue GetScrollbarDarkShadowColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ScrollbarDarkShadowColor);
        }

        void SetScrollbarDarkShadowColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ScrollbarDarkShadowColor = value;
        }

        JsValue GetScrollbarFaceColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ScrollbarFaceColor);
        }

        void SetScrollbarFaceColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ScrollbarFaceColor = value;
        }

        JsValue GetScrollbarHighlightColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ScrollbarHighlightColor);
        }

        void SetScrollbarHighlightColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ScrollbarHighlightColor = value;
        }

        JsValue GetScrollbarShadowColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ScrollbarShadowColor);
        }

        void SetScrollbarShadowColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ScrollbarShadowColor = value;
        }

        JsValue GetScrollbarTrackColor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ScrollbarTrackColor);
        }

        void SetScrollbarTrackColor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ScrollbarTrackColor = value;
        }

        JsValue GetStroke(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Stroke);
        }

        void SetStroke(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Stroke = value;
        }

        JsValue GetStrokeDasharray(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeDasharray);
        }

        void SetStrokeDasharray(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeDasharray = value;
        }

        JsValue GetStrokeDashoffset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeDashoffset);
        }

        void SetStrokeDashoffset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeDashoffset = value;
        }

        JsValue GetStrokeLinecap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeLinecap);
        }

        void SetStrokeLinecap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeLinecap = value;
        }

        JsValue GetStrokeLinejoin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeLinejoin);
        }

        void SetStrokeLinejoin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeLinejoin = value;
        }

        JsValue GetStrokeMiterlimit(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeMiterlimit);
        }

        void SetStrokeMiterlimit(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeMiterlimit = value;
        }

        JsValue GetStrokeOpacity(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeOpacity);
        }

        void SetStrokeOpacity(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeOpacity = value;
        }

        JsValue GetStrokeWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.StrokeWidth);
        }

        void SetStrokeWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.StrokeWidth = value;
        }

        JsValue GetTableLayout(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TableLayout);
        }

        void SetTableLayout(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TableLayout = value;
        }

        JsValue GetTextAlign(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextAlign);
        }

        void SetTextAlign(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextAlign = value;
        }

        JsValue GetTextAlignLast(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextAlignLast);
        }

        void SetTextAlignLast(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextAlignLast = value;
        }

        JsValue GetTextAnchor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextAnchor);
        }

        void SetTextAnchor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextAnchor = value;
        }

        JsValue GetTextAutospace(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextAutospace);
        }

        void SetTextAutospace(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextAutospace = value;
        }

        JsValue GetTextDecoration(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextDecoration);
        }

        void SetTextDecoration(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextDecoration = value;
        }

        JsValue GetTextIndent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextIndent);
        }

        void SetTextIndent(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextIndent = value;
        }

        JsValue GetTextJustify(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextJustify);
        }

        void SetTextJustify(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextJustify = value;
        }

        JsValue GetTextOverflow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextOverflow);
        }

        void SetTextOverflow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextOverflow = value;
        }

        JsValue GetTextShadow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextShadow);
        }

        void SetTextShadow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextShadow = value;
        }

        JsValue GetTextTransform(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextTransform);
        }

        void SetTextTransform(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextTransform = value;
        }

        JsValue GetTextUnderlinePosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TextUnderlinePosition);
        }

        void SetTextUnderlinePosition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TextUnderlinePosition = value;
        }

        JsValue GetTop(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Top);
        }

        void SetTop(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Top = value;
        }

        JsValue GetTransform(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Transform);
        }

        void SetTransform(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Transform = value;
        }

        JsValue GetTransformOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TransformOrigin);
        }

        void SetTransformOrigin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TransformOrigin = value;
        }

        JsValue GetTransformStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TransformStyle);
        }

        void SetTransformStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TransformStyle = value;
        }

        JsValue GetTransition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Transition);
        }

        void SetTransition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Transition = value;
        }

        JsValue GetTransitionDelay(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TransitionDelay);
        }

        void SetTransitionDelay(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TransitionDelay = value;
        }

        JsValue GetTransitionDuration(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TransitionDuration);
        }

        void SetTransitionDuration(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TransitionDuration = value;
        }

        JsValue GetTransitionProperty(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TransitionProperty);
        }

        void SetTransitionProperty(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TransitionProperty = value;
        }

        JsValue GetTransitionTimingFunction(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.TransitionTimingFunction);
        }

        void SetTransitionTimingFunction(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.TransitionTimingFunction = value;
        }

        JsValue GetUnicodeBidi(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.UnicodeBidi);
        }

        void SetUnicodeBidi(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.UnicodeBidi = value;
        }

        JsValue GetVerticalAlign(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.VerticalAlign);
        }

        void SetVerticalAlign(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.VerticalAlign = value;
        }

        JsValue GetVisibility(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Visibility);
        }

        void SetVisibility(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Visibility = value;
        }

        JsValue GetWhiteSpace(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.WhiteSpace);
        }

        void SetWhiteSpace(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.WhiteSpace = value;
        }

        JsValue GetWidows(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Widows);
        }

        void SetWidows(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Widows = value;
        }

        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Width);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Width = value;
        }

        JsValue GetWordBreak(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.WordBreak);
        }

        void SetWordBreak(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.WordBreak = value;
        }

        JsValue GetWordSpacing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.WordSpacing);
        }

        void SetWordSpacing(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.WordSpacing = value;
        }

        JsValue GetWordWrap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.WordWrap);
        }

        void SetWordWrap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.WordWrap = value;
        }

        JsValue GetWritingMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.WritingMode);
        }

        void SetWritingMode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.WritingMode = value;
        }

        JsValue GetZIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.ZIndex);
        }

        void SetZIndex(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.ZIndex = value;
        }

        JsValue GetZoom(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            return _engine.GetDomNode(reference.Zoom);
        }

        void SetZoom(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleDeclarationInstance>(Fail).RefCSSStyleDeclaration;
            var value = TypeConverter.ToString(argument);
            reference.Zoom = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSStyleDeclaration]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}