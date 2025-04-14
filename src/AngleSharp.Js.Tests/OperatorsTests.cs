namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class OperatorsTests
    {
        [Test]
        public async Task InOperatorExistingAttribute()
        {

            var result = await "'action' in document.createElement('form')".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task InOperatorNonExistingAttribute()
        {

            var result = await "'action' in document.createElement('div')".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task InOperatorInputWithinForm()
        {
            var result = await "'input1' in new DOMParser().parseFromString(`<form><input name='input1'/></form>`, 'text/html').body.firstChild".EvalScriptAsync();
            Assert.AreEqual("True", result);
        }

        [Test]
        public async Task InOperatorInputWithinDiv()
        {
            var result = await "'input1' in new DOMParser().parseFromString(`<div><input name='input1'/></div>`, 'text/html').body.firstChild".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

        [Test]
        public async Task InOperator_Issue104()
        {
            var result = await "'somethingThatDoesntExist' in document.createElement('form')".EvalScriptAsync();
            Assert.AreEqual("False", result);
        }

    }
}
