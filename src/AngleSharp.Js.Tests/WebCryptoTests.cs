//  crypto.subtle is not available on .NET Framework
#if !NETFRAMEWORK
namespace AngleSharp.Js.Tests
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The operations of crypto.subtle (Web Cryptography API), checked with the test vectors of
    /// the standards defining the algorithms where there are some.
    /// </summary>
    [TestFixture]
    public class WebCryptoTests
    {
        private const String Prelude = @"
            var subtle = crypto.subtle;
            function hex(buffer) { return Array.from(new Uint8Array(buffer)).map(function (b) { return (b < 16 ? '0' : '') + b.toString(16); }).join(''); }
            function bytes(text) { var r = new Uint8Array(text.length / 2); for (var i = 0; i < r.length; i++) { r[i] = parseInt(text.substr(2 * i, 2), 16); } return r; }
            function utf8(text) { return new TextEncoder().encode(text); }
        ";

        //  Runs the body of an async function; the result (or the name of the error) ends up as the title.
        private static async Task<String> Run(String body)
        {
            var cfg = Configuration.Default.WithJs().WithEventLoop();
            var script = $"{Prelude} (async function () {{ {body} }})().then(function (r) {{ document.title = 'ok:' + r; }}, function (e) {{ document.title = 'error:' + (e && e.name) + ':' + (e && e.message); }});";
            var document = await BrowsingContext.New(cfg).OpenAsync(m => m.Content($"<!doctype html><title></title><script>{script}</script>"));

            for (var i = 0; i < 500 && String.IsNullOrEmpty(document.Title); i++)
            {
                await Task.Delay(20);
            }

            return document.Title;
        }

        [Test]
        public async Task SubtleIsAvailable()
        {
            var result = await Run("return [typeof crypto.subtle, crypto.subtle === crypto.subtle, crypto.subtle instanceof SubtleCrypto, typeof CryptoKey].join();");
            Assert.AreEqual("ok:object,true,true,function", result);
        }

        [TestCase("SHA-1", "a9993e364706816aba3e25717850c26c9cd0d89d")]
        [TestCase("SHA-256", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]
        [TestCase("sha-384", "cb00753f45a35e8bb5a03d699ac65007272c32ab0eded1631a8b605a43ff5bed8086072ba1e7cc2358baeca134c825a7")]
        [TestCase("SHA-512", "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f")]
        public async Task DigestHashes(String name, String expected)
        {
            var result = await Run($"var d = await subtle.digest({{ name: '{name}' }}, utf8('abc')); return (d instanceof ArrayBuffer) + ',' + hex(d);");
            Assert.AreEqual("ok:true," + expected, result);
        }

        [Test]
        public async Task DigestTakesTheNameAlone()
        {
            var result = await Run("return hex(await subtle.digest('SHA-256', new Uint8Array(0).buffer));");
            Assert.AreEqual("ok:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", result);
        }

        [Test]
        public async Task UnknownAlgorithmRejectsWithNotSupportedError()
        {
            var result = await Run("return await subtle.digest('MD5', utf8('abc'));");
            Assert.That(result, Does.StartWith("error:NotSupportedError"));
        }

        [Test]
        public async Task HmacSignsAndVerifies()
        {
            //  RFC 4231, test case 2
            var result = await Run(@"
                var key = await subtle.importKey('raw', utf8('Jefe'), { name: 'HMAC', hash: 'SHA-256' }, false, ['sign', 'verify']);
                var data = utf8('what do ya want for nothing?');
                var signature = await subtle.sign('HMAC', key, data);
                var valid = await subtle.verify('HMAC', key, signature, data);
                var invalid = await subtle.verify('HMAC', key, signature, utf8('something else'));
                return [hex(signature), valid, invalid, key instanceof CryptoKey, key.type, key.algorithm.name, key.algorithm.hash.name, key.algorithm.length, key.usages.join('+'), key.extractable].join();");

            Assert.AreEqual("ok:5bdcc146bf60754e6a042426089575c75a003f089d2739839dec58b964ec3843,true,false,true,secret,HMAC,SHA-256,32,sign+verify,false", result);
        }

        [Test]
        public async Task GeneratedHmacKeyHasTheBlockSize()
        {
            var result = await Run(@"
                var key = await subtle.generateKey({ name: 'HMAC', hash: { name: 'SHA-512' } }, true, ['verify', 'sign']);
                var raw = await subtle.exportKey('raw', key);
                return [key.algorithm.length, raw.byteLength, key.usages.join('+')].join();");

            Assert.AreEqual("ok:1024,128,sign+verify", result);
        }

        [Test]
        public async Task Pbkdf2DerivesBits()
        {
            //  RFC 6070 (SHA-1) and the PBKDF2-HMAC-SHA256 vector of RFC 7914
            var result = await Run(@"
                var key = await subtle.importKey('raw', utf8('password'), 'PBKDF2', false, ['deriveBits']);
                var sha1 = await subtle.deriveBits({ name: 'PBKDF2', salt: utf8('salt'), iterations: 2, hash: 'SHA-1' }, key, 160);
                var other = await subtle.importKey('raw', utf8('passwd'), 'PBKDF2', false, ['deriveBits']);
                var sha256 = await subtle.deriveBits({ name: 'PBKDF2', salt: utf8('salt'), iterations: 1, hash: 'SHA-256' }, other, 256);
                return hex(sha1) + ',' + hex(sha256);");

            Assert.AreEqual("ok:ea6c014dc72d6f8ccd1ed92ace1d41f0d8de8957,55ac046e56e3089fec1691c22544b605f94185216dde0465e68b9d57c20dacbc", result);
        }

        [Test]
        public async Task HkdfDerivesBits()
        {
            //  RFC 5869, test case 1
            var result = await Run(@"
                var key = await subtle.importKey('raw', bytes('0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b'), 'HKDF', false, ['deriveBits']);
                var bits = await subtle.deriveBits({ name: 'HKDF', hash: 'SHA-256', salt: bytes('000102030405060708090a0b0c'), info: bytes('f0f1f2f3f4f5f6f7f8f9') }, key, 336);
                return hex(bits);");

            Assert.AreEqual("ok:3cb25f25faacd57a90434f64d0362f2a2d2d0a90cf1a5a4c5db02d56ecc4c5bf34007208d5b887185865", result);
        }

        [Test]
        public async Task DeriveKeyGivesAUsableKey()
        {
            var result = await Run(@"
                var base = await subtle.importKey('raw', utf8('secret'), 'PBKDF2', false, ['deriveKey']);
                var key = await subtle.deriveKey({ name: 'PBKDF2', salt: utf8('salt'), iterations: 1000, hash: 'SHA-256' }, base, { name: 'AES-GCM', length: 256 }, true, ['encrypt', 'decrypt']);
                var iv = new Uint8Array(12);
                var encrypted = await subtle.encrypt({ name: 'AES-GCM', iv: iv }, key, utf8('hello'));
                var decrypted = await subtle.decrypt({ name: 'AES-GCM', iv: iv }, key, encrypted);
                return [key.algorithm.name, key.algorithm.length, new TextDecoder().decode(decrypted)].join();");

            Assert.AreEqual("ok:AES-GCM,256,hello", result);
        }

        [Test]
        public async Task AesCbcEncrypts()
        {
            //  NIST SP 800-38A, F.2.1 (the first block; PKCS#7 padding adds a second)
            var result = await Run(@"
                var key = await subtle.importKey('raw', bytes('2b7e151628aed2a6abf7158809cf4f3c'), 'AES-CBC', false, ['encrypt', 'decrypt']);
                var iv = bytes('000102030405060708090a0b0c0d0e0f');
                var encrypted = await subtle.encrypt({ name: 'AES-CBC', iv: iv }, key, bytes('6bc1bee22e409f96e93d7e117393172a'));
                var decrypted = await subtle.decrypt({ name: 'AES-CBC', iv: iv }, key, encrypted);
                return [hex(encrypted).substr(0, 32), encrypted.byteLength, hex(decrypted)].join();");

            Assert.AreEqual("ok:7649abac8119b246cee98e9b12e9197d,32,6bc1bee22e409f96e93d7e117393172a", result);
        }

        [Test]
        public async Task AesCtrEncrypts()
        {
            //  NIST SP 800-38A, F.5.1
            var result = await Run(@"
                var key = await subtle.importKey('raw', bytes('2b7e151628aed2a6abf7158809cf4f3c'), 'AES-CTR', false, ['encrypt', 'decrypt']);
                var params = { name: 'AES-CTR', counter: bytes('f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff'), length: 64 };
                var encrypted = await subtle.encrypt(params, key, bytes('6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e51'));
                var decrypted = await subtle.decrypt(params, key, encrypted);
                return [hex(encrypted), hex(decrypted)].join();");

            Assert.AreEqual("ok:874d6191b620e3261bef6864990db6ce9806f66b7970fdff8617187bb9fffdff,6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e51", result);
        }

        [Test]
        public async Task AesGcmRejectsTamperedData()
        {
            var result = await Run(@"
                var key = await subtle.generateKey({ name: 'AES-GCM', length: 128 }, false, ['encrypt', 'decrypt']);
                var iv = crypto.getRandomValues(new Uint8Array(12));
                var encrypted = new Uint8Array(await subtle.encrypt({ name: 'AES-GCM', iv: iv, additionalData: utf8('header') }, key, utf8('hello')));
                var length = encrypted.length;
                encrypted[0] ^= 1;
                try {
                    await subtle.decrypt({ name: 'AES-GCM', iv: iv, additionalData: utf8('header') }, key, encrypted);
                    return 'decrypted';
                } catch (e) {
                    return length + ',' + e.name + ',' + (e instanceof DOMException);
                }");

            Assert.AreEqual("ok:21,OperationError,true", result);
        }

        [Test]
        public async Task AesKwWrapsKeys()
        {
            //  RFC 3394, 4.1
            var result = await Run(@"
                var kek = await subtle.importKey('raw', bytes('000102030405060708090a0b0c0d0e0f'), 'AES-KW', false, ['wrapKey', 'unwrapKey']);
                var key = await subtle.importKey('raw', bytes('00112233445566778899aabbccddeeff'), 'AES-CBC', true, ['encrypt']);
                var wrapped = await subtle.wrapKey('raw', key, kek, 'AES-KW');
                var unwrapped = await subtle.unwrapKey('raw', wrapped, kek, 'AES-KW', 'AES-CBC', true, ['encrypt']);
                return [hex(wrapped), hex(await subtle.exportKey('raw', unwrapped))].join();");

            Assert.AreEqual("ok:1fa68b0a8112b447aef34bd8fb5a7b829d3e862371d2cfe5,00112233445566778899aabbccddeeff", result);
        }

        [Test]
        public async Task JwkKeysCanBeWrapped()
        {
            var result = await Run(@"
                var wrapping = await subtle.generateKey({ name: 'AES-GCM', length: 256 }, false, ['wrapKey', 'unwrapKey']);
                var key = await subtle.generateKey({ name: 'HMAC', hash: 'SHA-256' }, true, ['sign']);
                var iv = new Uint8Array(12);
                var wrapped = await subtle.wrapKey('jwk', key, wrapping, { name: 'AES-GCM', iv: iv });
                var unwrapped = await subtle.unwrapKey('jwk', wrapped, wrapping, { name: 'AES-GCM', iv: iv }, { name: 'HMAC', hash: 'SHA-256' }, true, ['sign']);
                var a = await subtle.exportKey('jwk', key);
                var b = await subtle.exportKey('jwk', unwrapped);
                return [a.k === b.k, b.kty, b.alg, b.ext, b.key_ops.join('+')].join();");

            Assert.AreEqual("ok:true,oct,HS256,true,sign", result);
        }

        [TestCase("P-256", 64)]
        [TestCase("P-384", 96)]
        [TestCase("P-521", 132)]
        public async Task EcdsaSignsAndVerifies(String curve, Int32 signatureLength)
        {
            var result = await Run($@"
                var pair = await subtle.generateKey({{ name: 'ECDSA', namedCurve: '{curve}' }}, true, ['sign', 'verify']);
                var params = {{ name: 'ECDSA', hash: 'SHA-256' }};
                var signature = await subtle.sign(params, pair.privateKey, utf8('message'));
                var valid = await subtle.verify(params, pair.publicKey, signature, utf8('message'));
                var invalid = await subtle.verify(params, pair.publicKey, signature, utf8('other'));
                return [signature.byteLength, valid, invalid, pair.publicKey.type, pair.privateKey.type, pair.publicKey.usages.join('+'), pair.privateKey.usages.join('+'), pair.publicKey.algorithm.namedCurve].join();");

            Assert.AreEqual($"ok:{signatureLength},true,false,public,private,verify,sign,{curve}", result);
        }

        [Test]
        public async Task EcKeysAreExportedAndImported()
        {
            var result = await Run(@"
                var pair = await subtle.generateKey({ name: 'ECDSA', namedCurve: 'P-256' }, true, ['sign', 'verify']);
                var jwk = await subtle.exportKey('jwk', pair.privateKey);
                var raw = await subtle.exportKey('raw', pair.publicKey);
                var spki = await subtle.exportKey('spki', pair.publicKey);
                var pkcs8 = await subtle.exportKey('pkcs8', pair.privateKey);
                var privateKey = await subtle.importKey('jwk', jwk, { name: 'ECDSA', namedCurve: 'P-256' }, false, ['sign']);
                var fromPkcs8 = await subtle.importKey('pkcs8', pkcs8, { name: 'ECDSA', namedCurve: 'P-256' }, false, ['sign']);
                var fromRaw = await subtle.importKey('raw', raw, { name: 'ECDSA', namedCurve: 'P-256' }, true, ['verify']);
                var fromSpki = await subtle.importKey('spki', spki, { name: 'ECDSA', namedCurve: 'P-256' }, true, ['verify']);
                var params = { name: 'ECDSA', hash: 'SHA-384' };
                var signature = await subtle.sign(params, privateKey, utf8('m'));
                var other = await subtle.sign(params, fromPkcs8, utf8('m'));
                return [jwk.kty, jwk.crv, jwk.d.length, raw.byteLength, new Uint8Array(raw)[0],
                    await subtle.verify(params, fromRaw, signature, utf8('m')),
                    await subtle.verify(params, fromSpki, other, utf8('m')),
                    hex(await subtle.exportKey('raw', fromSpki)) === hex(raw)].join();");

            Assert.AreEqual("ok:EC,P-256,43,65,4,true,true,true", result);
        }

        [Test]
        public async Task EcdhAgreesOnTheSameSecret()
        {
            var result = await Run(@"
                var alice = await subtle.generateKey({ name: 'ECDH', namedCurve: 'P-384' }, false, ['deriveBits', 'deriveKey']);
                var bob = await subtle.generateKey({ name: 'ECDH', namedCurve: 'P-384' }, false, ['deriveBits']);
                var a = await subtle.deriveBits({ name: 'ECDH', public: bob.publicKey }, alice.privateKey, 384);
                var b = await subtle.deriveBits({ name: 'ECDH', public: alice.publicKey }, bob.privateKey, 384);
                var key = await subtle.deriveKey({ name: 'ECDH', public: bob.publicKey }, alice.privateKey, { name: 'AES-GCM', length: 256 }, true, ['encrypt']);
                var raw = await subtle.exportKey('raw', key);
                return [a.byteLength, hex(a) === hex(b), hex(raw) === hex(a).substr(0, 64), alice.publicKey.usages.length].join();");

            Assert.AreEqual("ok:48,true,true,0", result);
        }

        [TestCase("RSASSA-PKCS1-v1_5", "{ name: 'RSASSA-PKCS1-v1_5' }", "RS256")]
        [TestCase("RSA-PSS", "{ name: 'RSA-PSS', saltLength: 32 }", "PS256")]
        public async Task RsaSignsAndVerifies(String name, String parameters, String alg)
        {
            var result = await Run($@"
                var pair = await subtle.generateKey({{ name: '{name}', modulusLength: 2048, publicExponent: new Uint8Array([1, 0, 1]), hash: 'SHA-256' }}, true, ['sign', 'verify']);
                var signature = await subtle.sign({parameters}, pair.privateKey, utf8('message'));
                var jwk = await subtle.exportKey('jwk', pair.publicKey);
                var spki = await subtle.exportKey('spki', pair.publicKey);
                var fromJwk = await subtle.importKey('jwk', jwk, {{ name: '{name}', hash: 'SHA-256' }}, false, ['verify']);
                var fromSpki = await subtle.importKey('spki', spki, {{ name: '{name}', hash: 'SHA-256' }}, false, ['verify']);
                return [signature.byteLength,
                    await subtle.verify({parameters}, fromJwk, signature, utf8('message')),
                    await subtle.verify({parameters}, fromSpki, signature, utf8('other')),
                    jwk.alg, jwk.e, pair.publicKey.algorithm.modulusLength, Array.from(pair.publicKey.algorithm.publicExponent).join('.')].join();");

            Assert.AreEqual($"ok:256,true,false,{alg},AQAB,2048,1.0.1", result);
        }

        [Test]
        public async Task RsaOaepEncrypts()
        {
            var result = await Run(@"
                var pair = await subtle.generateKey({ name: 'RSA-OAEP', modulusLength: 2048, publicExponent: new Uint8Array([1, 0, 1]), hash: 'SHA-256' }, true, ['encrypt', 'decrypt']);
                var encrypted = await subtle.encrypt({ name: 'RSA-OAEP' }, pair.publicKey, utf8('secret'));
                var pkcs8 = await subtle.exportKey('pkcs8', pair.privateKey);
                var privateKey = await subtle.importKey('pkcs8', pkcs8, { name: 'RSA-OAEP', hash: 'SHA-256' }, false, ['decrypt']);
                var decrypted = await subtle.decrypt({ name: 'RSA-OAEP' }, privateKey, encrypted);
                var jwk = await subtle.exportKey('jwk', pair.privateKey);
                return [encrypted.byteLength, new TextDecoder().decode(decrypted), jwk.alg, typeof jwk.qi].join();");

            Assert.AreEqual("ok:256,secret,RSA-OAEP-256,string", result);
        }

        [TestCase("await subtle.exportKey('raw', await subtle.generateKey({ name: 'AES-CBC', length: 128 }, false, ['encrypt']))", "InvalidAccessError")]
        [TestCase("var k = await subtle.generateKey({ name: 'AES-CBC', length: 128 }, false, ['encrypt']); await subtle.decrypt({ name: 'AES-CBC', iv: new Uint8Array(16) }, k, new Uint8Array(16))", "InvalidAccessError")]
        [TestCase("var k = await subtle.generateKey({ name: 'AES-CBC', length: 128 }, false, ['encrypt']); await subtle.encrypt({ name: 'AES-CTR', counter: new Uint8Array(16), length: 64 }, k, new Uint8Array(16))", "InvalidAccessError")]
        [TestCase("await subtle.generateKey({ name: 'AES-GCM', length: 128 }, false, [])", "SyntaxError")]
        [TestCase("await subtle.generateKey({ name: 'AES-GCM', length: 128 }, false, ['sign'])", "SyntaxError")]
        [TestCase("await subtle.generateKey({ name: 'AES-GCM', length: 100 }, false, ['encrypt'])", "OperationError")]
        [TestCase("await subtle.importKey('raw', new Uint8Array(15), 'AES-GCM', false, ['encrypt'])", "DataError")]
        [TestCase("await subtle.importKey('raw', utf8('x'), 'PBKDF2', true, ['deriveBits'])", "SyntaxError")]
        [TestCase("await subtle.importKey('jwk', { kty: 'oct', k: 'AAAAAAAAAAAAAAAAAAAAAA', alg: 'A256GCM' }, 'AES-GCM', false, ['encrypt'])", "DataError")]
        [TestCase("await subtle.importKey('jwk', { kty: 'oct', k: 'AAAAAAAAAAAAAAAAAAAAAA', ext: false }, 'AES-GCM', true, ['encrypt'])", "DataError")]
        [TestCase("await subtle.importKey('pem', new Uint8Array(16), 'AES-GCM', false, ['encrypt'])", "TypeError")]
        [TestCase("await subtle.generateKey({ name: 'AES-GCM', length: 128 }, false, ['launch'])", "TypeError")]
        [TestCase("await subtle.generateKey({ name: 'ECDSA', namedCurve: 'P-192' }, false, ['sign'])", "NotSupportedError")]
        [TestCase("await subtle.generateKey({ name: 'HMAC' }, false, ['sign'])", "TypeError")]
        [TestCase("await subtle.sign('SHA-256', null, new Uint8Array(1))", "NotSupportedError")]
        public async Task FailuresRejectWithTheErrorOfTheStandard(String body, String expected)
        {
            var result = await Run(body + "; return 'done';");
            Assert.That(result, Does.StartWith("error:" + expected + ":"));
        }

        [Test]
        public async Task FailuresRejectRatherThanThrow()
        {
            var result = await Run(@"
                var p = subtle.digest('MD5', new Uint8Array(1));
                var isPromise = p instanceof Promise;
                try { await p; } catch (e) { return isPromise + ',' + e.name; }");

            Assert.AreEqual("ok:true,NotSupportedError", result);
        }
    }
}
#endif
