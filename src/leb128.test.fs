require test.fs
require leb128.fs

create n1 0x01 c,
create n2 0x80 c, 0x01 c,
create n3 0x85 c, 0x34 c,

n1 LEB128->u 0x0001  test-equal
n2 LEB128->u 0x0100  test-equal
n3 LEB128->u 0x3405  test-equal
