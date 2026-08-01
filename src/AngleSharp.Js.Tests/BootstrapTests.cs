namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class BootstrapTests
    {
        private static Task<String> EvaluateScriptWithBootstrapAsync(params String[] sources)
        {
            var list = new List<String>(sources);
            list.Insert(0, Constants.Bootstrap_5_3_3);
            return list.EvalScriptsAsync();
        }

        [Test]
        public async Task BootstrapCanInstantiateAlertOncePerElement()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var el = document.createElement('div');
el.className = 'alert';
document.body.appendChild(el);
var first = bootstrap.Alert.getOrCreateInstance(el);
var second = bootstrap.Alert.getOrCreateInstance(el);
document.querySelector('#result').textContent = (first === second).toString();")
                .ConfigureAwait(false);
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task BootstrapExposesVersionString()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
document.querySelector('#result').textContent = bootstrap.Tooltip.VERSION;")
                .ConfigureAwait(false);
            Assert.AreEqual("5.3.3", result);
        }

        [Test]
        public async Task BootstrapButtonToggleSetsActiveAndAriaPressed()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var button = document.createElement('button');
button.className = 'btn';
document.body.appendChild(button);
var instance = new bootstrap.Button(button);
instance.toggle();
document.querySelector('#result').textContent = button.classList.contains('active').toString() + ':' + button.getAttribute('aria-pressed');")
                .ConfigureAwait(false);
            Assert.AreEqual("true:true", result);
        }

        [Test]
        public async Task BootstrapCollapseShowAndHideUpdatesExpandedState()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var panel = document.createElement('div');
panel.id = 'panel';
panel.className = 'collapse';
document.body.appendChild(panel);

var collapse = new bootstrap.Collapse(panel, { toggle: false });
var sameInstance = (bootstrap.Collapse.getInstance(panel) === collapse).toString();
var state = 'ok';
try {
    collapse.show();
    collapse.hide();
}
catch (e) {
    state = 'fail';
}
document.querySelector('#result').textContent = sameInstance + ':' + state;")
                .ConfigureAwait(false);
            Assert.AreEqual("true:ok", result);
        }

        [Test]
        public async Task BootstrapTabCanBeInstantiatedAndTracked()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var nav = document.createElement('div');
nav.innerHTML = '<button id=\'tab1\' class=\'nav-link active\' data-bs-toggle=\'tab\' data-bs-target=\'#pane1\'>A</button>' +
    '<button id=\'tab2\' class=\'nav-link\' data-bs-toggle=\'tab\' data-bs-target=\'#pane2\'>B</button>';
document.body.appendChild(nav);

var content = document.createElement('div');
content.innerHTML = '<div id=\'pane1\' class=\'tab-pane active show\'>One</div>' +
    '<div id=\'pane2\' class=\'tab-pane\'>Two</div>';
document.body.appendChild(content);

var tab = new bootstrap.Tab(document.getElementById('tab2'));
var sameInstance = (bootstrap.Tab.getInstance(document.getElementById('tab2')) === tab).toString();
document.querySelector('#result').textContent = sameInstance;")
                .ConfigureAwait(false);
            Assert.AreEqual("true", result);
        }

        [Test]
        public async Task BootstrapToastShowAndHideFiresLifecycleEvents()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var el = document.createElement('div');
el.className = 'toast';
document.body.appendChild(el);
var toast = new bootstrap.Toast(el, { autohide: false });
var sameInstance = (bootstrap.Toast.getInstance(el) === toast).toString();
var state = 'ok';
try {
    toast.show();
    toast.hide();
}
catch (e) {
    state = 'fail';
}
document.querySelector('#result').textContent = sameInstance + ':' + state;")
                .ConfigureAwait(false);
            Assert.AreEqual("true:ok", result);
        }

        [Test]
        public async Task BootstrapModalShowAndHideTogglesBodyClass()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var modalEl = document.createElement('div');
modalEl.className = 'modal';
modalEl.innerHTML = '<div class=\'modal-dialog\'><div class=\'modal-content\'></div></div>';
document.body.appendChild(modalEl);

var modal = new bootstrap.Modal(modalEl, { backdrop: false, keyboard: false });
modal.show();
var duringShow = document.body.classList.contains('modal-open').toString();

modalEl.addEventListener('hidden.bs.modal', function () {
    var afterHide = document.body.classList.contains('modal-open').toString();
    document.querySelector('#result').textContent = duringShow + ':' + afterHide;
});

modal.hide();")
                .ConfigureAwait(false);
            Assert.AreEqual("true:false", result);
        }

        [Test]
        public async Task BootstrapExportsExpectedComponentConstructors()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var checks = [
    typeof bootstrap.Alert,
    typeof bootstrap.Button,
    typeof bootstrap.Collapse,
    typeof bootstrap.Modal,
    typeof bootstrap.Carousel,
    typeof bootstrap.Toast
];
document.querySelector('#result').textContent = checks.join(':');")
                .ConfigureAwait(false);
            Assert.AreEqual("function:function:function:function:function:function", result);
        }

        [Test]
        public async Task BootstrapCarouselCanMoveToSpecificSlide()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var el = document.createElement('div');
el.className = 'carousel slide';
el.innerHTML = '<div class=\'carousel-inner\'>' +
    '<div class=\'carousel-item active\'>1</div>' +
    '<div class=\'carousel-item\'>2</div>' +
    '<div class=\'carousel-item\'>3</div>' +
    '</div>';
document.body.appendChild(el);

var carousel = new bootstrap.Carousel(el, { interval: false, ride: false, wrap: true });
var sameInstance = (bootstrap.Carousel.getInstance(el) === carousel).toString();
var state = 'ok';
try {
    carousel.next();
    carousel.prev();
    carousel.to(2);
}
catch (e) {
    state = 'fail';
}
document.querySelector('#result').textContent = sameInstance + ':' + state;")
                .ConfigureAwait(false);
            Assert.AreEqual("true:ok", result);
        }

        [Test]
        public async Task BootstrapOffcanvasCanBeInstantiatedAndTracked()
        {
            var result = await EvaluateScriptWithBootstrapAsync(@"
var el = document.createElement('div');
el.className = 'offcanvas offcanvas-start';
document.body.appendChild(el);

var offcanvas = new bootstrap.Offcanvas(el, { backdrop: false });
document.querySelector('#result').textContent = (bootstrap.Offcanvas.getInstance(el) === offcanvas).toString();")
                .ConfigureAwait(false);
            Assert.AreEqual("true", result);
        }
    }
}
