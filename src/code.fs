require leb128.fs
require section.fs

\ Control Instructions
$02 constant block.instr
$0B constant block.end
$0C constant br

\ 5.4.5 Numeric Instructions
$41 constant i32.const
\ 0x42 constant i64.const

$6A constant i32.add

\ Variable Instructions
$20 constant local.get

\ Block return types.
$40 constant VOID

: wasm-compile-i32.const ( addr1 -- addr2 )
  char+                \ Skip op-code.
  dup i32@             \ Read value.
  u-to-s compile-file  \ Write constant to output.
  compile-cr
;

: wasm-compile-i32.add ( addr1 -- addr2 )
  char+                 \ Skip op-code.
  s" add" compile-file 
  compile-cr
;

: wasm-compile-br
  char+       \ FSkip op-code.
  u32@        \ Read nesting level label.
  number-generator swap -
  { jmp }
  s" depth block" compile-file 
  jmp u-to-s compile-file 
  s"  - " compile-file 
  compile-cr          

  \ Restoring the stack                   
  block-type VOID = if
    \ The block has no return values
    s" 0 ?do drop loop" compile-file compile-cr
  else
    \ The block has one return value
    s" 1- 0 ?do drop loop" compile-file compile-cr
  endif 
;

: wasm-compile-block.end
  char+                 \ Skip op-code.
;

: wasm-compile-local.get
  char+                 \ Skip op-code.
  char+
;

: wasm-compile-block.instr ( addr1 code-end -- addr2 ) 
  { code-end }
  char+                  \ Skip op-code.
  dup c@ { block-type }  \ Read block type.                     
  char+                  \ Forward.
  \ TODO: sp@ sp! to read and store stack pointer.
  \ TODO: store depth on return stack,
  s" depth >r" write-ln        
  block-type 
  code-end 
  wasm-compile-block 
;

: wasm-compile-block ( addr1 btype ngen code-end -- addr2 )
  recursive
  { block-type number-generator code-end }
  begin
    dup c@ ~~     \ Reading instruction
    case
      i32.const   of wasm-compile-i32.const endof
      i32.add     of wasm-compile-i32.add endof
      br          of 
                    char+       \ Forward to nesting level label.
                    u32@        \ Read nesting level label.
                    number-generator swap -
                    { jmp }
                    s" depth block" compile-file 
                    jmp u-to-s compile-file 
                    s"  - " compile-file 
                    compile-cr          

                    \ Restoring the stack                   
                    block-type VOID = if
                      \ The block has no return values
                      s" 0 ?do drop loop" compile-file compile-cr
                    else
                      \ The block has one return value
                      s" 1- 0 ?do drop loop" compile-file compile-cr
                    endif 
                  endof
                  \ TODO: what does exit do?
      block.end   of wasm-compile-block.end exit endof
      local.get   of wasm-compile-local.get endof
      block.instr of 
                    char+                  \ Skip op-code.
                    dup c@ { block-type }  \ Read block type.                     
                    char+ 
                    s" depth { block" compile-file
                    number-generator u-to-s compile-file 
                    s"  } " compile-file
                    compile-cr          
                    block-type 
                    number-generator 1+ 
                    code-end 
                    wasm-compile-block 
                  endof
    endcase
  dup code-end >= until
;

: wasm-compile-code-section ( addr1 -- addr2 )
  dup c@ CODE-SECTION <> if 
    s" Expecting code section" exception
    throw
  endif
  char+                 \ Forward to code block size.
  dup u32@              \ Read size field.
  drop                  \ We are not intersted in the size.
  dup u32@              \ Read code vec.
  drop                  \ Currently only one is supported.
  dup u32@              \ Read code size.
  \ over +                   \ Compute the end of the code block.
  { code-size }            
  dup code-size +       \ Compute end of code block.
  1- { code-end }       \ Subtract one because it will be used as index.
  \ dup u32@ +               \ Skip locals.
  dup u32@              \ Reading locals.
  \ TODO: support for locals.
  { number-of-locals }  \ Assuming no locals.
  number-of-locals +    \ Skipping locals because locals have always one byte size.
  VOID 0 code-end       \ Push block type, initial counter and end of code address.
  wasm-compile-block    \ Compile instructions.
;