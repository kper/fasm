4 Constant version-block-size ( because 4 bytes )

Create version-buffer version-block-size allot
Create cmp-version-bytes 1 , 0 , 0 , 0 

: check_version ( -- flag )
    version-buffer version-block-size
    cmp-version-bytes version-block-size
    compare
    0= ( mein Problem ist dass ich hier -1 aber in allen anderen Faellen thrown moechte )
;

: parse_version_bytes 
    version-buffer version-block-size read_bytes
    check_version
    ;