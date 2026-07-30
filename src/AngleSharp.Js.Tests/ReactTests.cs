namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class ReactTests
    {
        private static Task<String> EvaluateScriptWithReactAsync(params String[] sources)
        {
            var list = new List<String>(sources);
            list.Insert(0, Constants.React16_8_6);
            return list.EvalScriptsAsync();
        }

        private static Task<String> EvaluateScriptWithReactDomAsync(params String[] sources)
        {
            var list = new List<String>(sources);
            list.Insert(0, Constants.React16_8_6);
            list.Insert(1, Constants.ReactDom16_8_6);
            return list.EvalScriptsAsync();
        }

        private static String SetResult(String eval) =>
            $"document.querySelector('#result').textContent = {eval};";

        [Test]
        public async Task ReactJustCreateAVdomElement()
        {
            var result = await EvaluateScriptWithReactAsync(SetResult("React.createElement('div').type"))
                .ConfigureAwait(false);
            Assert.AreEqual("div", result);
        }

        [Test]
        public async Task ReactDomCanRenderSimpleApplication()
        {
            var script = @"var app = document.createElement('div');
app.id = 'app';
document.body.appendChild(app);
var element = React.createElement('span', null, 'Hello');
ReactDOM.render(element, app);
document.querySelector('#result').innerHTML = app.innerHTML;";

            var result = await EvaluateScriptWithReactDomAsync(script)
                .ConfigureAwait(false);
            Assert.AreEqual("<span>Hello</span>", result);
        }

        [Test]
        public async Task ReactDomCanRenderMappedList()
        {
            var script = @"var app = document.createElement('div');
app.id = 'app';
document.body.appendChild(app);
var items = ['one', 'two', 'three'];
var list = React.createElement('ul', null,
    items.map(function (item) {
        return React.createElement('li', { key: item }, item.toUpperCase());
    })
);
ReactDOM.render(list, app);
document.querySelector('#result').innerHTML = app.innerHTML;";

            var result = await EvaluateScriptWithReactDomAsync(script)
                .ConfigureAwait(false);
            Assert.AreEqual("<ul><li>ONE</li><li>TWO</li><li>THREE</li></ul>", result);
        }

        [Test]
        public async Task ReactDomCanRerenderWithUpdatedProps()
        {
            var script = @"var app = document.createElement('div');
app.id = 'app';
document.body.appendChild(app);
var View = function (props) {
    return React.createElement('strong', null, props.value);
};
ReactDOM.render(React.createElement(View, { value: 'v1' }), app);
ReactDOM.render(React.createElement(View, { value: 'v2' }), app);
document.querySelector('#result').innerHTML = app.innerHTML;";

            var result = await EvaluateScriptWithReactDomAsync(script)
                .ConfigureAwait(false);
            Assert.AreEqual("<strong>v2</strong>", result);
        }

        [Test]
        public async Task ReactDomCanRenderContextProviderConsumer()
        {
            var script = @"var app = document.createElement('div');
app.id = 'app';
document.body.appendChild(app);

var ThemeContext = React.createContext('default');
var view = React.createElement(
    ThemeContext.Provider,
    { value: 'from-provider' },
    React.createElement(ThemeContext.Consumer, null, function (value) {
        return React.createElement('em', null, value);
    })
);

ReactDOM.render(view, app);
document.querySelector('#result').innerHTML = app.innerHTML;
";

            var result = await EvaluateScriptWithReactDomAsync(script)
                .ConfigureAwait(false);
            Assert.AreEqual("<em>from-provider</em>", result);
        }

        [Test]
        public async Task ReactDomCanUseHooksForState()
        {
            var script = @"var app = document.createElement('div');
app.id = 'app';
document.body.appendChild(app);

var Counter = function () {
    var state = React.useState(8);
    var count = state[0];

    return React.createElement('span', null, count.toString());
};

ReactDOM.render(React.createElement(Counter), app);
document.querySelector('#result').innerHTML = app.innerHTML;";

            var result = await EvaluateScriptWithReactDomAsync(script)
                .ConfigureAwait(false);
            Assert.AreEqual("<span>8</span>", result);
        }
    }
}
