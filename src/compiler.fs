require io.fs
require magic_bytes.fs
require version_bytes.fs

: parsing_module
    parse_magic_bytes 
    parse_version_bytes
    ;
