: magic-number-expect-byte ( addr1 c -- addr1 )
  \g Tests whether the given byte c is equal to the byte 
  \g at addr1. Returns an exception else.
  over c@ <> if 
    s" Invalid WASM file. Wrong magic number." exception 
    throw 
  endif
;

: validate-magic-number ( addr1 -- addr2 )
  \g Validates the magic number '\0asm'.
  0x00 magic-number-expect-byte char+
  0x61 magic-number-expect-byte char+
  0x73 magic-number-expect-byte char+
  0x6D magic-number-expect-byte char+
;