require test.fs

create n1 0x01 c,
create n2 0x80 c, 0x01 c,
create n3 0x85 c, 0x34 c,
create n4 0xC8 c, 0x01 c,
create n5 0xAC c, 0x02 c,
create n6 0xC7 c, 0xF7 c, 0xF0 c, 0x01 c,
create n7 0xCF c, 0xEF c, 0xDD c, 0x04 c,

create c1 0x7F c,
create c2 0xFF c, 0x00 c,
create c3 0x81 c, 0x7F c,


\ TODO: Test new address
n1 LEB128->u 1       test-equal drop
n2 LEB128->u 128     test-equal drop
n3 LEB128->u 6661    test-equal drop
n4 LEB128->u 200     test-equal drop
n5 LEB128->u 300     test-equal drop
n6 LEB128->u 3947463 test-equal drop
n7 LEB128->u 9926607 test-equal drop

c1 LEB128->u 127     test-equal drop
c2 LEB128->u 127     test-equal drop
c3 LEB128->u 16257   test-equal drop

c1 LEB128->s -1      test-equal drop
c2 LEB128->s 127     test-equal drop
c3 LEB128->s -127    test-equal drop

bye