\ Returns the lower 7 bits of an integer.
: [7..0] ( n1 -- n2 ) 0x7F and ;

\ Shifts u1 to the left by n bits.
: << ( u1 n -- u2 ) lshift ;

\ Shift 1 to the left by n bits.
: 1<< ( n -- n2 ) 1 swap << ;

\ Shifts u1 to the right by n bits.
: >> ( u1 n -- u2 ) rshift ;
