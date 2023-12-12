: .cs 
  .s 
; immediate

: cs-swap ( d0/o0 d1/o1 -- d1/o1 d0/o0 )
  \g Swaps the top two control stack frames.
  \
  { x0 x1 x2 x3 }  \ Top control stack frame.
  { y0 y1 y2 y3 }  \ Bottom control stack frame.
  x0 x1 x2 x3
  y0 y1 y2 y3      \ Swap.
;

: wasm-block ( compilation: -- dest orig )
  \g Starts a new WASM block.
  \g 
  \g               +-- wasm-br
  \g   +-- ahead   |
  \g   |   begin <-+
  \g   |   ahead --+
  \g   +-> then    |
  \g               +-> wasm-end
  \
  \ TODO: remember stack depth.
  postpone ahead  \ On block entry jump to then.
  postpone begin  \ Jump target for branches inside the block.
  cs-swap         \ Put block entry origin on top.
  postpone ahead  \ Jump to the end of the block.
  cs-swap         \ Put block entry origin on top.
  postpone then   \ Block entry jump target.
; immediate

: wasm-loop ( compilation: -- dest orig )
  \g Starts a new WASM loop.
  \g
  \g           +-- wasm-br
  \g           |
  \g   begin <-+
  \g   <???> --+
  \g           |
  \g           +-> (don't care)
  \
  \ TODO: remember stack depth.
  postpone begin  \ Jump target for branches inside the block.
  0 0 0 dest      \ Phony control flow stack item.
; immediate

: wasm-br ( compilation: lvl -- )
  \g WASm unconditional jump.
  \
  2 * 1 + cs-pick   \ wasm-block and wasm-loop both add two frames
                    \ on the control flow stack. Pick the dest-orig 
                    \ frame pair according to nesting level. Then 
                    \ take the desp part of the pair.
  postpone again    \ Jump to the start of the block or loop.
; immediate

: wasm-end ( compilation: dest orig -- )
  \g Ends a WASM block or loop.
  \
  dup dest = if     \ Phony control flow stack item is of type dest.
    cs-drop         \ Drop the phony control flow stack item. We 
                    \ never jump to the end of loop blocks.
  else
    postpone then   \ Jump target for the WASM block.
  endif
  cs-drop           \ Remove destinatin control frame.
; immediate

\ : main
\   s" Start Program " type cr
\   wasm-block
\     s" Start Block 0 " type cr
\     wasm-block
\       s" Start Block 1 " type cr
\       [ 0 ] wasm-br
\       s" End Block 1 " type cr
\     wasm-end
\     s" End Block 0 " type cr
\   wasm-end
\   s" End Program " type cr  
\ ;

\ : main
\   wasm-loop
\     s" outer loop " type cr
\     wasm-loop
\       s" inner loop " type cr
\       [ 0 ] wasm-br
\       s" inner loop - !!! " type cr
\     wasm-end
\     s" outer-loop - !!! " type cr
\     \ wasm-br
\   wasm-end
\   s" but this we should see " type cr  
\ ;

\ main

bye