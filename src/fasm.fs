require compiler.fs

: wasm-run 
  next-arg
  wasm-parse
  wasm-compile
;

wasm-run

bye