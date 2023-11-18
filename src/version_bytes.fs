\ 4 Constant version-block-size ( because 4 bytes )

\ Create version-buffer version-block-size allot
\ Create cmp-version-bytes 1 , 0 , 0 , 0 

\ : check_version ( -- )
\     version-buffer version-block-size
\     cmp-version-bytes version-block-size
\     compare
\     0= ( mein Problem ist dass ich hier -1 aber in allen anderen Faellen thrown moechte )
\     drop
\ ;

\ : parse_version_bytes 
\     version-buffer version-block-size read_bytes
\     check_version
\     ;

\ TODO: Make it more general
: version-expect-byte ( addr1 c -- addr1 )
  \g Tests whether the given byte c is equal to the byte 
  \g at addr1. Returns an exception else.
  over c@ <> if 
    s" Invalid WASM file. Wrong version." exception 
    throw 
  endif
;

: validate-version ( addr1 -- addr2 )
  \g Validates the WASM file version.
  0x01 version-expect-byte char+
  0x00 version-expect-byte char+
  0x00 version-expect-byte char+
  0x00 version-expect-byte char+
;