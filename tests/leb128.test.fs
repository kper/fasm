require test.fs

create n1 0x01 c,
create n2 0x80 c, 0x01 c,
create n3 0x85 c, 0x34 c,
create n4 0xC8 c, 0x01 c,
create n5 0xAC c, 0x02 c,
create n6 0x


\ TODO: Test new address
n1 LEB128->u 0x0001  test-equal
n2 LEB128->u 0x0100  test-equal
n3 LEB128->u 0x3405  test-equal
n4 LEB128->u 200     test-equal
n5 LEB128->u 300     test-equal

\ TODO: Test signed values

bye