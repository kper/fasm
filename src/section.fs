require leb128.fs

0x0 constant CUSTOM-SECTION
0x1 constant TYPE-SECTION
0x2 constant IMPORT-SECTION
0x3 constant FUNCTION-SECTION
0x4 constant TABLE-SECTION
0x5 constant MEMORY-SECTION
0x6 constant GLOBAL-SECTION
0x7 constant EXPORT-SECTION
0x8 constant START-SECTION
0x9 constant ELEMENT-SECTION
0xA constant CODE-SECTION
0xB constant DATA-SECTION

: skip-section { u } ( addr1 u -- addr2 )
  \g Skips section u if it is present.
  dup c@ u = if 
    char+       \ Forward to section size.
    dup u32@    \ Read size field.
    chars +     \ Forward size bytes.
  else
    u           \ Section did not match.
                \ Put u back on the stack 
                \ for the next section test.
  endif
;