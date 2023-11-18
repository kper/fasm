require test.fs

create wrong-magic-number   0x01 c, 0x61 c, 0x73 c, 0x6D c,
create correct-magic-number 0x00 c, 0x61 c, 0x73 c, 0x6D c,

\ TODO: test for expected exception
\ wrong-magic-number validate-magic-number 

correct-magic-number validate-magic-number 
correct-magic-number 4 chars +
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