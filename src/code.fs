require leb128.fs
require section.fs

create blocks 256 cells allot
create arity 256 cells allot

$1 constant IS_BLOCK
$2 constant IS_LOOP

\ Control Instructions
$2 constant block.instr
$0c constant br

\ 5.4.5 Numeric Instructions
$41 constant i32.const
\ 0x42 constant i64.const

$6A constant i32.add

\ Variable Instructions
$20 constant local.get

: wasm-compile-i32.const ( addr1 -- addr2 )
  char+       \ Forward to constant value.
  dup i32@    \ Read value.
  ~~
  u-to-s 
  s" movq $" compile-file compile-file s" , %rax" compile-file compile-cr 
  s" pushq %rax" compile-file compile-cr
  compile-cr
;

: wasm-compile-i32.add ( addr1 -- addr2 )
  char+
  s" " compile-file compile-cr
  s" popq %r8" compile-file compile-cr
  s" popq %rax" compile-file compile-cr
  s" addq %r8, %rax" compile-file compile-cr
  s" pushq %rax" compile-file compile-cr
;

: wasm-compile-block ( number-generator addr end-instruction-ptr -- addr2 )
  recursive
  { number-generator end-instruction-ptr }
  begin
    dup c@ ~~     \ Reading instruction
    case
      i32.const   of wasm-compile-i32.const endof
      i32.add     of wasm-compile-i32.add endof
      br          of 
                  char+       \ Forward to constant value.
                  u32@        \ Read label.
                  number-generator 1- swap -
                  { jmp }

                  \ TODO Restoring the stack
                  
                  blocks jmp cells + @ IS_BLOCK = if
                    s" jmp then_block" compile-file jmp u-to-s compile-file compile-cr
                  else
                    s" jmp block" compile-file jmp u-to-s compile-file compile-cr
                  then

                  endof
      11          of char+ exit endof
      local.get   of char+ char+ endof
      block.instr of 
                  char+ \ Read instruction
                  dup c@ \ Read blocktype 
                  { block-type }
                  char+ 

                  s" block" compile-file
                  number-generator u-to-s compile-file s" :" compile-file compile-cr          

                  IS_BLOCK blocks number-generator cells + !

                  block-type $40 = if
                    0 arity number-generator cells + !
                  else
                    1 arity number-generator cells + !
                  then

                  number-generator 1+ end-instruction-ptr wasm-compile-block 

                  s" then_block" compile-file
                  number-generator u-to-s compile-file s" :" compile-file compile-cr          

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
  { end-instruction-ptr }
  dup u32@                      \ Reading locals
  { number-of-locals }          \ Assuming no locals TODO
  number-of-locals +            \ Skipping locals because locals have always one byte size

  s" main:" compile-file compile-cr
  s" pushq %rbp" compile-file compile-cr

  0 end-instruction-ptr wasm-compile-block    
  s" popq %rbp" compile-file compile-cr
  s" ret" compile-file compile-cr
;