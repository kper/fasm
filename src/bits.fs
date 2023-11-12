: [7..0] ( n1 -- n2 ) 
  \g Returns the lower 7 bits of an integer.
  0x7F and ;

: << ( u1 n -- u2 ) 
  \g Shifts u1 to the left by n bits.
  lshift ;

: 1<< ( n -- n2 ) 
  \g Shift 1 to the left by n bits.
  1 swap << ;

: >> ( u1 n -- u2 ) 
  \g Shifts u1 to the right by n bits.
  rshift ;
