0 Value fd-in
0 Value fd-out

s" ./simple.wat" r/o bin open-file throw Value fd-in

4 Constant magic-block-size ( because 4 bytes )
4 Constant version-block-size ( because 4 bytes )

Create magic-buffer magic-block-size allot
Create cmp-magic-bytes 109 , 97 , 115 , 109
Create cmp-version-bytes 1 , 0 , 0 , 0 
Create version-buffer version-block-size allot

: read_bytes ( addr n -- )
    fd-in read-file throw drop drop drop ;

: read_bytes_from_mem_into_stack ( addr u1 -- [u2])
    { addr max }
    max 0 DO addr i + c@ LOOP 
    ;

: check_magic ( -- flag )
    magic-buffer magic-block-size 
    cmp-magic-bytes magic-block-size
    compare 
    0= ( mein Problem ist dass ich hier -1 aber in allen anderen Faellen thrown moechte )
    ;

: check_version ( -- flag )
    version-buffer version-block-size
    cmp-version-bytes version-block-size
    compare
    0= ( mein Problem ist dass ich hier -1 aber in allen anderen Faellen thrown moechte )
;

: parse_magic_bytes
    magic-buffer magic-block-size read_bytes
    check_magic
    ;

: parse_version_bytes 
    version-buffer version-block-size read_bytes
    check_version
    ;

: parsing_module
    parse_magic_bytes 
    ;
