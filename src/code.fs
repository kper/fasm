require leb128.fs
require section.fs

\ 5.4.5 Numeric Instructions
$41 constant i32.const
\ 0x42 constant i64.const

$6A constant i32.add

: wasm-compile-i32.const ( addr1 -- addr2 )
  char+       \ Forward to constant value.
  dup i32@    \ Read value.
  . cr        \ TODO: Write to file.
;

: wasm-compile-i32.add ( addr1 -- addr2 )
  s" add" type cr          \ TODO: Write to file.
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
  drop            \ We are not interested in the function size
  dup u32@        \ Reading locals
  drop            \ Assuming no locals TODO
  dup c@              \ Reading first instruction
  case
    i32.const of wasm-compile-i32.const endof
    i32.add   of wasm-compile-i32.add endof
    char+     \ Skip byte
  endcase
;