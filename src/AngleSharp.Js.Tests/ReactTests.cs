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
    }
}
