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