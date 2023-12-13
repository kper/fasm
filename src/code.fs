require leb128.fs
require section.fs

\ Instruction op-codes.
$02 constant block.begin
$03 constant loop.begin
$0B constant block.end
$0C constant br
$20 constant local.get
$41 constant i32.const
\ 0x42 constant i64.const
$6A constant i32.add

\ Block return types.
$40 constant VOID

: wasm-compile-block.begin ( addr1 -- addr2 ) 
  char+                     \ Skip op-code.
  dup c@                    \ Load block argument return type.
  VOID = if 0 else 1 endif  \ Push number of arguments. VOID has none.
  mun->out                  \ Write the number of arguments to output.              
  char+                     \ Forward.
  s" wasm-block" ln->out
;

\ TODO: incomplete?
: wasm-compile-loop.begin ( addr1 -- addr2 ) 
  char+                     \ Skip op-code.
  dup c@                    \ Load block argument return type.
  VOID = if 0 else 1 endif  \ Push number of arguments. VOID has none.
  mun->out                  \ Write the number of arguments to output.              
  char+                     \ Forward.
  s" wasm-loop" ln->out 
;

: wasm-compile-block.end ( addr1 -- addr2 ) 
  char+                 \ Skip op-code.
  s" wasm-end" ln->out
;

: wasm-compile-br ( addr1 -- addr2 ) 
  char+                 \ Skip op-code.
  s" [ "      str->out  \ Wrap nesting level in compile-time block.
  u32@        num->out  \ Write nesting level to output.
  s" ] "      str->out  \ End of compile-time block.
  s" wasm-br" ln->out
;

: wasm-compile-local.get
  char+                 \ Skip op-code.
  char+
;

: wasm-compile-i32.const ( addr1 -- addr2 )
  char+              \ Skip op-code.
  dup i32@ num->out  \ Write constant to output.
;

: wasm-compile-i32.add ( addr1 -- addr2 )
  char+            \ Skip op-code.
  s" add" ln->out
;

: wasm-compile-instructions ( addr1 code-end -- addr2 )
  { code-end }
  begin
    dup c@ ~~     \ Reading instruction
    case
      block.begin of wasm-compile-block.begin endof
      loop.begin  of wasm-compile-loop.begin  endof
                  \ TODO: what does exit do?
      block.end   of wasm-compile-block.end exit endof
      br          of wasm-compile-br          endof
      local.get   of wasm-compile-local.get   endof
      i32.const   of wasm-compile-i32.const   endof
      i32.add     of wasm-compile-i32.add     endof
    endcase
  dup code-end >= until
;

\ TODO: support for locals.
: wasm-compile-code-section ( addr1 -- addr2 )
  dup c@ CODE-SECTION <> if 
    s" Expecting code section" exception
    throw
  endif
  char+                   \ Forward to code block size.
  dup u32@                \ Read size field.
  drop                    \ We are not intersted in the size.
  dup u32@                \ Read code vec.
  drop                    \ Currently only one is supported.
  dup u32@                \ Read code size.
  over + 1- { code-end }  \ Compute the end of the code block.
  dup u32@                \ Read number of locals.
  +                       \ Skip all locals. Each local has szie one byte.
  code-end wasm-compile-instructions
;
