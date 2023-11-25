require leb128.fs
require section.fs

\ Control Instructions
$2 constant block.instr

\ 5.4.5 Numeric Instructions
$41 constant i32.const
\ 0x42 constant i64.const

$6A constant i32.add

\ Variable Instructions
$20 constant local.get

: wasm-compile-i32.const ( addr1 -- addr2 )
  char+       \ Forward to constant value.
  dup i32@    \ Read value.
  . cr        \ TODO: Write to file.
;

: wasm-compile-i32.add ( addr1 -- addr2 )
  char+
  s" add" type cr          \ TODO: Write to file.
;

: wasm-compile-block ( addr end-instruction-ptr -- addr2 )
  recursive
  { end-instruction-ptr }
  begin
    dup c@ ~~     \ Reading instruction
    case
      i32.const   of wasm-compile-i32.const endof
      i32.add     of wasm-compile-i32.add endof
      11          of char+ exit endof
      local.get   of char+ char+ endof
      block.instr of 
                  char+ \ Read instruction
                  char+ \ Read blocktype 

                  s" depth >r" type cr          \ TODO: Write to file.

                  end-instruction-ptr wasm-compile-block 

                  s" depth r> - remove-nth" type cr          \ TODO: Write to file.

                  endof
    endcase

  dup end-instruction-ptr >=
  until
;

: wasm-compile-code-section ( addr1 -- addr2 )
  dup c@ CODE-SECTION <> if 
    s" Expecting code section" exception
    throw
  endif
  char+           \ Forward to code block size.
  dup u32@        \ Read size field.
  drop            \ We are not intersted in the size.
  dup u32@        \ Read code vec
  drop            \ Currently only one is supported
  dup u32@        \ Read code size
  { code-size }            
  dup code-size + \ Compute end of code block
  1-              \ Subtract one because it will be used as index
  ~~
  { end-instruction-ptr }
  dup u32@                      \ Reading locals
  { number-of-locals }          \ Assuming no locals TODO
  number-of-locals +            \ Skipping locals because locals have always one byte size

  end-instruction-ptr wasm-compile-block    
;