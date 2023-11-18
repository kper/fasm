require test.fs

\ TODO: test for expected exception
\ s" ./wasm/wrong-magic.wasm" wasm-parse wasm-compile

\ TODO: compare output source
s" ./wasm/simple.wasm" wasm-parse wasm-compile

bye