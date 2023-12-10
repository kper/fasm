require test.fs

\ TODO: test for expected exception
\ s" ./wasm/wrong-magic.wasm" wasm-parse wasm-compile

\ TODO: compare output source
s" ./wasm/simpler.wasm" wasm-parse wasm-compile

bye
