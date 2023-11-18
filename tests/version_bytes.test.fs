require test.fs

create wrong-version   0x01 c, 0x00 c, 0x00 c, 0x01 c,
create correct-version 0x01 c, 0x00 c, 0x00 c, 0x00 c,

\ TODO: test for expected exception
\ wrong-version validate-version 

correct-version validate-version
correct-version 4 chars +
test-equal

\ : read_bytes
\     ( do nothing )
\ ;

\ require magic_bytes.fs

\ create test 109 , 97 , 115 , 109

\ : test 
\     assert( test check_magic )
\ ;

bye