require utils.fs
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

: wasm-compile-header ( -- )
  \g Compiles imports, globals and the 
  \g main colon-definition.
  \
  s" require wasm-runtime.fs" ln->out
  s" : main" ln->out
;

: wasm-compile-footer ( -- )
  \g Compiles the end of the main 
  \g colon-definition and calls it.
  \
  s" ;" ln->out
  s" main" ln->out
;

: wasm-compile ( addr -- )
  \g Compiles a WASM file to a FORTH source file.
  \
  wasm-compile-header
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
  ( CODE-SECTION ) wasm-compile-code-section
  DATA-SECTION     skip-section
  wasm-compile-footer
;
