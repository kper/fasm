256 constant RTS-DEPTH  \ Runtime stack depth.

\ Runtime stack of stack pointers.
create wasm-rts-sp    RTS-DEPTH cells allot  
\ Runtime stack of block arities.
create wasm-rts-arity RTS-DEPTH cells allot  
\ WASM runtime stack pointer.
create wasm-rtsp              1 cells allot  

0 wasm-rtsp !  \ Initialize pointer.

\ : .cs 
\   .s 
\ ; immediate

\ : wasm-debug
\   s" Stack:     " type
\   .s cr
\   s" RTSP:      " type
\   wasm-rtsp @ . cr
\   s" RTS Arity: " type
\   wasm-rts-arity @ . cr
\   s" RTS SP:    " type
\   wasm-rts-sp @ . cr
\   cr
\ ;

: wasm-rtsp++ ( -- )
  \g Increment the WASM runtime stack pointer.
  wasm-rtsp @ cell+ wasm-rtsp !
;

: wasm-rtsp-- ( -- )
  \g Decrement the WASM runtime stack pointer.
  wasm-rtsp @ cell- wasm-rtsp !
;

: wasm-rts-sp@ ( -- sp )
  \g Push the stack pointer stored at the current position
  \g of the WASM runtime stack pointer.
  wasm-rts-sp 
  wasm-rtsp @ cell- 
  + @
;

: wasm-rts-arity@ ( -- arity )
  \g Push the arity stored at the current position
  \g of the WASM runtime stack pointer.
  wasm-rts-arity 
  wasm-rtsp @ cell- 
  + @
;

: wasm-store-stack ( arity -- )
  \g Store the current stack pointer and the block aritiy
  \g on the WASM runtime stack.
  wasm-rts-arity wasm-rtsp @ + !
  sp@ wasm-rts-sp wasm-rtsp @ + !
  wasm-rtsp++
;

: wasm-skip-levels ( lvl -- )
  \g Restore WASM runtime stack pointer lvl items. If lvl is 0 
  \g the pointer remains at its current position.
  \
  wasm-rtsp @   \ Load current pointer to WASM runtime stack.
  swap cells -  \ Drop lvl items.
  wasm-rtsp !   \ Update pointer to WASM runtime stack.
;

: wasm-restore-stack ( v -- epsilon | v )
  \g Restores the original stack position. If the block arity
  \g is zeronon-zero staches the TOS item to the retur stack
  \g before it restores the stack position and then pushes the
  \g stashed value back onto the stack.
  \
  wasm-rts-arity@ 0= if
    wasm-rts-sp@ sp!  \ Restore stack pointer.
  else
    >r                \ Stash top item to return.
    wasm-rts-sp@ sp!  \ Restore stack pointer.
    r>                \ Restore top item to return.
  endif
;

: cs-swap ( d0/o0 d1/o1 -- d1/o1 d0/o0 )
  \g Swaps the top two control stack frames.
  \
  { x0 x1 x2 x3 }  \ Top control stack frame.
  { y0 y1 y2 y3 }  \ Bottom control stack frame.
  x0 x1 x2 x3
  y0 y1 y2 y3      \ Swap.
;

: wasm-block ( compilation: -- dest orig ; runtime: arity -- )
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
  postpone wasm-store-stack
  postpone ahead  \ On block entry jump to then.
  postpone begin  \ Jump target for branches inside the block.
  cs-swap         \ Put block entry origin on top.
  postpone ahead  \ Jump to the end of the block.
  cs-swap         \ Put block entry origin on top.
  postpone then   \ Block entry jump target.
; immediate

: wasm-loop ( compilation: -- dest orig ; runtime: arity -- )
  \g Starts a new WASM loop.
  \g
  \g           +-- wasm-br
  \g           |
  \g   begin <-+
  \g   <???> --+
  \g           |
  \g           +-> (don't care)
  \
  postpone wasm-store-stack
  postpone begin  \ Jump target for branches inside the block.
  0 0 0 dest      \ Phony control flow stack item.
; immediate

: wasm-br ( compilation: lvl -- ; runtime: lvl -- )
  \g WASM unconditional jump. Restores the original stack position
  \g and pushes the stack items to return, if any, back onto the 
  \g stack before it jumps to the beginning of the block or loop.
  \
  2 * 1 + cs-pick   \ wasm-block and wasm-loop both add two frames
                    \ on the control flow stack. Pick the dest-orig 
                    \ frame pair according to nesting level. Then 
                    \ take the dest part of the pair.
  postpone wasm-skip-levels
  postpone wasm-restore-stack
  postpone again    \ Jump to the start of the block or loop.
; immediate

: wasm-br-if ( compilation: lvl -- ; runtime: b -- )
  \g WASM conditinal jump. If TOS is non-zero then jump. Restores 
  \g the original stack position and pushes the stack items to 
  \g return, if any, back onto the stack before it jumps to the 
  \g beginning of the block or loop.
  \
  postpone if
  2 * 1 + cs-pick   \ wasm-block and wasm-loop both add two frames
                    \ on the control flow stack. Pick the dest-orig 
                    \ frame pair according to nesting level. Then 
                    \ take the dest part of the pair.
  postpone wasm-skip-levels
  postpone wasm-restore-stack
  postpone again    \ Jump to the start of the block or loop.
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
  cs-drop           \ Remove destination control frame.
  \ TODO: wasm-restore-stack?
  postpone wasm-rtsp--
; immediate
