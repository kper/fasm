require io.fs
require magic_bytes.fs
require version_bytes.fs

: parsing_module
  parse_magic_bytes 
  parse_version_bytes
;

: compile ( str -- )
  \g Reads the WASM file passed as the sole argument
  \g and compiles it to a FORTH source file.
  \
  slurp-file \ Reads the entire file into memory and 
             \ pushes an address and the file length 
             \ onto the stack.
  drop       \ We dont need the file size. All blocks
             \ start with their respective size.
  
;