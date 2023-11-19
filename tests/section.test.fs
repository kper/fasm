require test.fs

\ TODO: Bug too many addresses on stack after skip-section perhaps because of u32@?

: test-skip-section
  s" ./wasm/simpler.wasm" 
  wasm-parse
  dup { x }                     \ Put the source origin int a local variable.
  \ x . cr                        \ Print source origin.
  validate-magic-number
  validate-version

  dup x 8 chars + test-equal    \ Test if we are at the position where the first section starts.
  dup c@ 0x01 test-equal        \ Sould be pointing at type section ID.

  CUSTOM-SECTION skip-section   \ This should be a no-op.
  dup x 8 chars + test-equal
  dup c@ 0x01 test-equal


  TYPE-SECTION skip-section
  dup x 15 chars + test-equal   \ Type section is 2 + 5 byte long (plus 8 bytes for the magic 
                                \ and version numbers)
  dup c@ 0x03 test-equal        \ Should be pointing at function section ID.

  FUNCTION-SECTION skip-section
  dup x 19 chars + test-equal   \ Function section is 2 + 2 bytes.
  dup c@ 0x0A test-equal        \ Should be pointing at function section ID.

  CODE-SECTION skip-section
  dup x 30 chars + test-equal   \ Code section is 2 + 9 bytes.
;

test-skip-section

bye