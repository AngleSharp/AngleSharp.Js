# HTML5test fixture

This fixture was retrieved from `https://html5test.com/` on 2026-07-27:

- `https://html5test.com/`
- `https://html5test.com/scripts/base.js`
- `https://html5test.com/scripts/8/engine.js`
- `https://html5test.com/scripts/8/data.js`
- `https://html5test.com/assets/detect.html`
- `https://html5test.com/assets/csp.html`

HTML5test is Copyright (c) 2010-2016 Niels Leenheer and is distributed under
the MIT license, reproduced in `index.html`.

The Cloudflare email-decoder script was removed from `index.html`. The remote
WhichBrowser detector was replaced by `whichbrowser-stub.js`, which reports a
clean browser and keeps the fixture deterministic. A wrapper around the
destructuring feature probe converts Jint's unsupported `eval` path into the
feature-test's expected `SyntaxError` (https://github.com/sebastienros/jint/issues/2825).
No test engine code was modified.
