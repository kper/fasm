require bits.fs

: LEB128->u { addr -- addr2 u }
  \g Decodes an LEB128 encoded unsigned varaible size integer beginning
  \g at addr. Pushes the c-addr2 pointing at the next byte after the end 
  \g of the encoded integer as well as the integer onto the stack. 
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

: u64@ ( c-addr1 -- c-addr2 u64 ) 
  \g Decodes the LEB128 encoded unsigned 32 bit integer beginning at 
  \g c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  LEB128->u ;

: s64@ ( c-addr1 -- c-addr2 s64 ) 
  \g Decodes a LEB128 encoded signed 64 bit integer beginning at 
  \g c-addr1 Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  64 LEB128->s ;

: i64@ ( c-addr1 -- c-addr2 i64 ) 
  \g Decodes a LEB128 encoded uninterpreted 64 bit integer beginning 
  \g at c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  64 LEB128->s ;

: u32@ ( c-addr1 -- c-addr2 u32 ) 
  \g Decodes a LEB128 encoded unsigned 32 bit integer beginning at 
  \g c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  LEB128->u ;

: s32@ ( c-addr1 -- c-addr2 s32 ) 
  \g Decodes a LEB128 encoded signed 32 bit integer beginning at 
  \g c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  32 LEB128->s ;

: i32@ ( c-addr1 -- c-addr2 i32 ) 
  \g Decodes a LEB128 encoded uninterpreted 32 bit integer beginning 
  \g at c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  32 LEB128->s ;

: i16@ ( c-addr1 -- c-addr2 i16 ) 
  \g Decodes a LEB128 encoded uninterpreted 16 bit integer beginning 
  \g at c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  16 LEB128->s ;

: i8@ ( c-addr1 -- c-addr2 i8 ) 
  \g Decodes a LEB128 encoded uninterpreted 8 bit integer beginning 
  \g at c-addr1. Pushes the c-addr2 pointing at the next byte after the 
  \g end of the encoded integer as well as the integer onto the stack. 
  8 LEB128->s ;

\ TODO: -> Tests

create n1 0x01 c,
create n2 0x80 c, 0x01 c,
create n3 0x85 c, 0x34 c,

\ n1 LEB128->u
n3 LEB128->u