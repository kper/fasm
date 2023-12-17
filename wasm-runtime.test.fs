require wasm-runtime.fs

: test1
  0 { cnt }
  s" Test 1" type cr
  0 wasm-block

  wasm-end

;

: main
  s" Start Program " type cr
  0 wasm-block
    s" Start Block 0 " type cr
    0 wasm-block
      s" Start Block 1 " type cr
      [ 0 ] wasm-br
      s" End Block 1 " type cr
    wasm-end
    s" End Block 0 " type cr
  wasm-end
  s" End Program " type cr  
;

\ : main
\   0 wasm-loop
\     s" outer loop " type cr
\     0 wasm-loop
\       s" inner loop " type cr
\       [ 0 ] wasm-br
\       s" inner loop - !!! " type cr
\     wasm-end
\     s" outer-loop - !!! " type cr
\   wasm-end
\   s" but this we should see " type cr  
\ ;

\ main

bye