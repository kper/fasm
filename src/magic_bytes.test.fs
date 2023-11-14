: read_bytes
    ( do nothing )
;

require magic_bytes.fs

create test 109 , 97 , 115 , 109

: test 
    assert( test check_magic )
;