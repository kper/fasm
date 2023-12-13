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

: wasm-block ( compilation: -- dest orig ; runtime: u -- )
  \g Starts a new WASM block. Each block has a fixed number
  \g of returned stack items. These are preserved before the
  \g remaining stack contents is discarded and popped on
  \g the stack afterwards.
  \g 
  \g               +-- wasm-br
  \g   +-- ahead   |
  \g   |   begin <-+
  \g   |   ahead --+
  \g   +-> then    |
  \g               +-> wasm-end
  \
  postpone sp@    \ Get the current stack pointer.
  postpone >r     \ Store the current stack pointer.
  postpone >r     \ Store the number of returned stack items.
  postpone ahead  \ On block entry jump to then.
  postpone begin  \ Jump target for branches inside the block.
  cs-swap         \ Put block entry origin on top.
  postpone ahead  \ Jump to the end of the block.
  cs-swap         \ Put block entry origin on top.
  postpone then   \ Block entry jump target.
; immediate

: wasm-loop ( compilation: -- dest orig ; runtime: u -- )
  \g Starts a new WASM loop.
  \g
  \g           +-- wasm-br
  \g           |
  \g   begin <-+
  \g   <???> --+
  \g           |
  \g           +-> (don't care)
  \
  postpone sp@    \ Get the current stack pointer.
  postpone >r     \ Store the current stack pointer.
  postpone >r     \ Store the number of returned stack items.
  postpone begin  \ Jump target for branches inside the block.
  0 0 0 dest      \ Phony control flow stack item.
; immediate

\ TODO: Support multiple return items.
: wasm-br ( compilation: lvl -- )
  \g WASM unconditional jump. Restores the original stack position
  \g and pushes the stack items to return if any before it jumps to
  \g the beginning of the block or loop.
  \
  2 * 1 + cs-pick   \ wasm-block and wasm-loop both add two frames
                    \ on the control flow stack. Pick the dest-orig 
                    \ frame pair according to nesting level. Then 
                    \ take the desp part of the pair.
  postpone r>       \ Load the number of stack items to return.
  postpone 0=
  postpone if 
  postpone r>       \ Load the original stack pointer.
  postpone sp!      \ Restore original stack position.
  postpone else
  postpone { val }  \ Backup returned item.
  postpone r>       \ Load the original stack pointer.
  postpone sp!      \ Restore original stack position.
  postpone val      \ Restore returned item.
  postpone endif  
  postpone again    \ Jump to the start of the block or loop.
; immediate

: wasm-br-if ( compilation: lvl -- ; runtime: b -- )
  \g WASM conditinal jump. If TOS is non-zero then jump.
  \
  postpone if
  wasm-br
  postpone endif
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
  postpone r>       \ Load the number of returned stack items.
  postpone drop     \ Drop them.
  postpone r>       \ Load the original stack pointer.
  postpone drop     \ Drop it.
; immediate

\ : main
\   s" Start Program " type cr
\   0 wasm-block
\     s" Start Block 0 " type cr
\     0 wasm-block
\       s" Start Block 1 " type cr
\       [ 0 ] wasm-br
\       s" End Block 1 " type cr
\     wasm-end
\     s" End Block 0 " type cr
\   wasm-end
\   s" End Program " type cr  
\ ;

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