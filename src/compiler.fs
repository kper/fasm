require magic_bytes.fs
require version_bytes.fs
require section.fs
require code.fs

\ Descriptor for the output file.
\ 0 value fd-out

: wasm-parse ( str -- addr )
  \g Reads the WASM file passed as the sole argument.
  \
  slurp-file \ Reads the entire file into memory and 
             \ pushes an address and the file length 
             \ onto the stack.
  drop       \ We don't need the file size. All blocks
             \ start with their respective sizes.
;

: open-output ( str -- wfileid )  
  \g Create and open a file to write to.
  w/o create-file throw 
  \ TODO: I was expecting throw to remove the status code.
  drop                    \ Drop status code.
;

: wasm-compile ( addr -- )
  \g Compiles a WASM filet to a FORTH source file.
  \
  validate-magic-number
  validate-version
  TYPE-SECTION     skip-section
  IMPORT-SECTION   skip-section
  FUNCTION-SECTION skip-section
  TABLE-SECTION    skip-section
  MEMORY-SECTION   skip-section
  GLOBAL-SECTION   skip-section
  EXPORT-SECTION   skip-section
  START-SECTION    skip-section
  ELEMENT-SECTION  skip-section
  wasm-compile-code-section
  \ CODE-SECTION     skip-section
  DATA-SECTION     skip-section
;
