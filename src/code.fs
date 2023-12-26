require leb128.fs
require section.fs

\ Instruction op-codes.
$02 constant block.begin
$03 constant loop.begin
$0B constant block.end
$0C constant br
$0D constant br.if
$20 constant local.get
$21 constant local.set
$22 constant local.tee
$23 constant global.get
$24 constant global.set
$41 constant i32.const
\ 0x42 constant i64.const
$45 constant i32.eqz
$46 constant i32.eq
$47 constant i32.ne
$48 constant i32.lts
$49 constant i32.ltu
$4a constant i32.gts
$4b constant i32.gtu
$4c constant i32.les
$4d constant i32.leu
$4e constant i32.ges
$4f constant i32.geu

$6A constant i32.add
$6b constant i32.sub
$6c constant i32.mul
$6d constant i32.divs
$6e constant i32.divu
$6f constant i32.rems
$70 constant i32.remu

$71 constant i32.and
$72 constant i32.or
$73 constant i32.xor
$74 constant i32.shl
$75 constant i32.shrs
$76 constant i32.shru
$77 constant i32.rotl
$78 constant i32.rotr

$0F constant return

\ Memory

$28 constant i32.load
$36 constant i32.store

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

: wasm-compile-br.if ( addr1 -- addr2 ) 
  char+                 \ Skip op-code.
  s" [ "      str->out  \ Wrap nesting level in compile-time block.
  u32@        num->out  \ Write nesting level to output.
  s" ] "      str->out  \ End of compile-time block.
  s" wasm-br-if" ln->out
;

: wasm-compile-local.get
  char+                 \ Skip op-code.
  u32@
  s" local-stack " str->out 
  num->out 
  s" cells + @" ln->out
;

: wasm-compile-local.set
  char+                 \ Skip op-code.
  u32@
  s" local-stack " str->out 
  num->out 
  s"  cells + !" ln->out
;

: wasm-compile-global.get
  char+                 \ Skip op-code.
  u32@
  s" global-stack " str->out 
  num->out 
  s" cells + @" ln->out
;

: wasm-compile-global.set
  char+                 \ Skip op-code.
  u32@
  s" global-stack " str->out 
  num->out 
  s"  cells + !" ln->out
;

: wasm-compile-local.tee
  char+                 \ Skip op-code.
  u32@
  s" dup local-stack " str->out 
  num->out 
  s" cells + !" ln->out
;

: wasm-compile-i32.const ( addr1 -- addr2 )
  char+              \ Skip op-code.
  dup i32@ num->out  \ Write constant to output.
;

: wasm-compile-i32.add ( addr1 -- addr2 )
  char+            \ Skip op-code.
  s" +" ln->out
;

: wasm-compile-i32.eqz ( addr1 -- addr2 )
  char+            \ Skip op-code.
  s" 0=" ln->out
;

: wasm-compile-i32.eq ( addr1 -- addr2 )
  char+
  s" =" ln->out
;

: wasm-compile-i32.ne ( addr1 -- addr2 )
  char+
  s" <>" ln->out
;

: wasm-compile-i32.lt_s ( addr1 -- addr2 )
  char+
  s" <" ln->out
;

: wasm-compile-i32.lt_u ( addr1 -- addr2 )
  char+
  s" u<" ln->out
;

: wasm-compile-i32.gt_s ( addr1 -- addr2 )
  char+
  s" >" ln->out
;

: wasm-compile-i32.gt_u ( addr1 -- addr2 )
  char+
  s" u>" ln->out
;

: wasm-compile-i32.le_s ( addr1 -- addr2 )
  char+
  s" <=" ln->out
;

: wasm-compile-i32.le_u ( addr1 -- addr2 )
  char+
  s" u>=" ln->out
;

: wasm-compile-i32.ge_s ( addr1 -- addr2 )
  char+
  s" >=" ln->out
;

: wasm-compile-i32.ge_u ( addr1 -- addr2 )
  char+
  s" u>=" ln->out
;

: wasm-compile-i32.sub ( addr1 -- addr2 )
  char+
  s" -" ln->out
;

: wasm-compile-i32.mul ( addr1 -- addr2 )
  char+
  s" *" ln->out
;

: wasm-compile-i32.div_s ( addr1 -- addr2 )
  char+
  s" /" ln->out
;

: wasm-compile-i32.div_u ( addr1 -- addr2 )
  char+
  s" bye" ln->out
;

: wasm-compile-i32.rem_s ( addr1 -- addr2 )
  char+
  s" mod" ln->out
;

: wasm-compile-i32.rem_u ( addr1 -- addr2 )
  char+
  s" bye" ln->out
;

: wasm-compile-i32.and ( addr1 -- addr2 )
  char+
  s" and" ln->out
;

: wasm-compile-i32.or ( addr1 -- addr2 )
  char+
  s" or" ln->out
;

: wasm-compile-i32.xor ( addr1 -- addr2 )
  char+
  s" xor" ln->out
;

: wasm-compile-i32.shl ( addr1 -- addr2 )
  char+
  s" lshift" ln->out
;

: wasm-compile-i32.shr_s ( addr1 -- addr2 )
  char+
  s" /2" ln->out
;

: wasm-compile-i32.shr_u ( addr1 -- addr2 )
  char+
  s" rshift" ln->out
;

: wasm-compile-i32.rotl ( addr1 -- addr2 )
  char+
  s" bye" ln->out
;

: wasm-compile-i32.rotr ( addr1 -- addr2 )
  char+
  s" bye" ln->out
;

: wasm-compile-i32.load ( addr1 -- addr2 )
  char+
  u32@ { align }
  u32@ { offset }
  offset num->out 
  s" add" ln->out
  s" memory add @" ln->out
;

: wasm-compile-i32.store ( addr1 -- addr2 )
  char+
  u32@ { align }
  u32@ { offset }
  offset num->out 
  s" add" ln->out
  s" memory add !" ln->out
;

: wasm-compile-instructions ( addr1 code-end -- addr2 )
  { code-end }
  begin
    dup c@ ~~     \ Reading instruction
    case
      block.begin of wasm-compile-block.begin endof
      loop.begin  of wasm-compile-loop.begin  endof
      block.end   of wasm-compile-block.end   endof
      br          of wasm-compile-br          endof
      br.if       of wasm-compile-br.if       endof
      local.get   of wasm-compile-local.get   endof
      local.set   of wasm-compile-local.set   endof
      local.tee   of wasm-compile-local.tee   endof
      global.get  of wasm-compile-global.get  endof
      global.set  of wasm-compile-global.set  endof
      i32.const   of wasm-compile-i32.const   endof
      i32.add     of wasm-compile-i32.add     endof
      i32.eqz     of wasm-compile-i32.eqz     endof
      i32.eq      of wasm-compile-i32.eq      endof
      i32.ne      of wasm-compile-i32.ne      endof
      i32.lts     of wasm-compile-i32.lt_s    endof
      i32.ltu     of wasm-compile-i32.lt_u    endof
      i32.gts     of wasm-compile-i32.gt_s    endof
      i32.gtu     of wasm-compile-i32.gt_u    endof
      i32.les     of wasm-compile-i32.le_s    endof
      i32.leu     of wasm-compile-i32.le_u    endof
      i32.ges     of wasm-compile-i32.ge_s    endof
      i32.geu     of wasm-compile-i32.ge_u    endof
      i32.sub     of wasm-compile-i32.sub     endof
      i32.mul     of wasm-compile-i32.mul     endof
      i32.divu    of wasm-compile-i32.div_u   endof
      i32.divs    of wasm-compile-i32.div_s   endof
      i32.rems    of wasm-compile-i32.rem_s   endof
      i32.remu    of wasm-compile-i32.rem_u   endof
      i32.and     of wasm-compile-i32.and     endof
      i32.or      of wasm-compile-i32.or      endof
      i32.xor     of wasm-compile-i32.xor     endof
      i32.shl     of wasm-compile-i32.shl     endof
      i32.shrs    of wasm-compile-i32.shr_s   endof
      i32.shru    of wasm-compile-i32.shr_u   endof 
      i32.rotl    of wasm-compile-i32.rotl    endof
      i32.rotr    of wasm-compile-i32.rotr    endof
      i32.store   of wasm-compile-i32.store   endof
      i32.load    of wasm-compile-i32.load    endof
      return      of char+                    endof
    endcase
  dup code-end >= until
;

: wasm-locals ( addr1 -- addr2 )
  u32@ drop char+ 
;

: vec-locals ( addr -- addr2 )
  u32@ 0
  ?do
    wasm-locals
  loop
;

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
  dup vec-locals          \ Read number of locals.
  code-end wasm-compile-instructions
;
