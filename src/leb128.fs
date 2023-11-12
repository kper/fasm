\ TODO: Requires postpone or ommediate magic or macro magic else 
\ TODO: the returns tack contains the return address from this function call.
\ Increments the top element on the return stack.
: r+ ( -- ) r> 1+ >r ;     

\ Returns the lower 7 bits of an integer.
: [7..0] ( n1 -- n2 ) 0x7F and ;

\ Shifts u1 to the left by n bits.
: << ( u1 n -- u2 ) lshift ;

\ Shift 1 to the left by n bits.
: 1<< ( n -- n2 ) 1 swap << ;

\ Shifts u1 to the right by n bits.
: >> ( u1 n -- u2 ) rshift ;

: LEB128->u { addr -- addr2 u }
  \g Decodes an LEB128 encoded unsigned varaible size integer beginning
  \g at addr and pushes the addr2 pointing at the end of the encoded 
  \g integer as well as the decoded unsigned integer u onto the stack. 
  \
  0 >r                    \ Initialize the counter c = 0 on the return stack.
  0 begin                 \ Initialize the value accumulator u = 0.
    addr r@ chars + c@    \ Load byte z.
    dup [7..0] r@ 8 * <<  \ Compute (z & 0x7f) << (c * 8).
    rot or swap           \ Update u = u | (z & 0x7f << (c * 8).
    r> 1+ >r              \ Increnent the counter c.
  0x80 and 0= until       \ Repeat if most significant bit is 1.
  addr r> + swap          \ Return the new address and the value.
;

: LEB128->s ( addr n -- addr2 s ) { n }
  LEB128->u dup n 1<< and 0<> if negate endif
;

\ WASM, p87
: LEB128->i ( addr n -- addr2 s ) LEB128->s ;

: LEB128->u64 ( c-addr1 -- c-addr2 u64 ) LEB128->u ;

: LEB128->s64 ( c-addr1 -- c-addr2 s64 ) 64 LEB128->s ;

: LEB128->i64 ( c-addr1 -- c-addr2 i64 ) 64 LEB128->i ;

: LEB128->u32 ( c-addr1 -- c-addr2 u32 ) LEB128->u ;

: LEB128->s32 ( c-addr1 -- c-addr2 s32 ) 32 LEB128->s ;

: LEB128->i32 ( c-addr1 -- c-addr2 i32 ) 32 LEB128->i ;

: LEB128->i16 ( c-addr1 -- c-addr2 i16 ) 16 LEB128->i ;

: LEB128->i8 ( c-addr1 -- c-addr2 i8 ) 
  \g Decodes a LEB128 encoded uninterpreted 8 bit integer beginning 
  \g at c-addr1 and pushes the c-addr2 pointing at the end of the 
  \g encoded integer as well as the decoded integer onto the stack. 
  8 LEB128->i ;

: u64@ ( c-addr1 -- c-addr2 u64 ) LEB128->u ;

: s64@ ( c-addr1 -- c-addr2 s64 ) 64 LEB128->s ;

: i64@ ( c-addr1 -- c-addr2 i64 ) 64 LEB128->i ;

: u32@ ( c-addr1 -- c-addr2 u32 ) LEB128->u ;

: s32@ ( c-addr1 -- c-addr2 s32 ) 32 LEB128->s ;

: i32@ ( c-addr1 -- c-addr2 i32 ) 32 LEB128->i ;

: i16@ ( c-addr1 -- c-addr2 i16 ) 16 LEB128->i ;

: i8@ ( c-addr1 -- c-addr2 i8 ) 
  \g Decodes a LEB128 encoded uninterpreted 8 bit integer beginning 
  \g at c-addr1 and pushes the c-addr2 pointing at the end of the 
  \g encoded integer as well as the decoded integer onto the stack. 
  8 LEB128->i ;

create n1 0x01 c,
create n2 0x80 c, 0x01 c,
create n3 0x85 c, 0x34 c,

\ n1 LEB128>cell
n3 LEB128>cell