: .cs 
  .s 
; immediate

: wasm-block 
  postpone ahead
  postpone begin
  1 cs-roll
  postpone then
; immediate

: wasm-loop
  postpone begin
; immediate

\ : wasm-br 
\   postpone ahead 
\ ; immediate

: wasm-br ( compilation: lvl -- )
  cs-pick
  postpone again 
; immediate

: wasm-end
  cs-drop
  \ dup dest = if 
  \   cs-drop
  \ endif
; immediate

: wasm-orig?
  dup dead-orig 1+ live-orig within
;

\ : wasm-block-end
\   dup dead-orig 1+ live-orig within 0= if
\     postpone then
\   endif 
\ ; immediate

\ This one loops forever.
\ : main
\   begin
\     s" Hello " type
\   again
\ ;

\ This skips ahead.
\ : main
\   s" we see this " type cr
\   ahead
\     s" should not print this " type cr
\   then
\   s" but this we should see " type cr
\ ;

\ : main
\   wasm-block
\     wasm-block
\       s" we see this " type cr
\       \ ..s
\       wasm-br
\       \ ..s
\       s" should not print this " type cr
\       \ ..s
\     wasm-block-end
\     \ ..s
\     s" should not print this as well " type cr
\     \ wasm-br
\   wasm-block-end
\   s" but this we should see " type cr  
\ ;

: main
  wasm-loop
    s" outer loop " type cr
    wasm-loop
      s" inner loop " type cr
      \ ..s
      [ 0 ] wasm-br
      \ ..s
      s" inner loop - !!! " type cr
      \ ..s
    wasm-end
    \ ..s
    s" outer-loop - !!! " type cr
    \ wasm-br
  wasm-end
  s" but this we should see " type cr  
;

main

bye