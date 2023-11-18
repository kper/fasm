\ 4 Constant magic-block-size ( because 4 bytes )

\ Create magic-buffer magic-block-size allot
\ Create cmp-magic-bytes 109 , 97 , 115 , 109

\ : check_magic ( -- )
\     magic-block-size 
\     cmp-magic-bytes magic-block-size
\     compare 
\     0= ( mein Problem ist dass ich hier -1 aber in allen anderen Faellen thrown moechte )
\     drop
\ ;

\ : parse_magic_bytes
\     magic-buffer magic-block-size read_bytes
\     magic-buffer check_magic
\ ;

: expect-byte ( addr1 c -- addr1 )
  over c@ <> if 
    s" Invalid WASM file. Wrong magic number." exception 
    throw 
  endif
;

: validate-magic-number ( addr1 -- addr2 )
  \g Validates the magic number '\0asm'.
  0x00 expect-byte char+
  0x61 expect-byte char+
  0x73 expect-byte char+
  0x6D expect-byte char+
;