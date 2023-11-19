require io.fs
require section.fs

create instruction 255
create payload 02 , 255 , 255 , 255
create n1 01 , 255

: check_if_0xFF { addr -- }
    addr 1
    instruction 1
    compare
    0=
;

: check_if_0xFF_v2 { addr -- }
    addr 1
    instruction 1
    compare
    0=
    addr 1 + ( increment it for the next iteration )
;

: test
    n1 ['] check_if_0xFF vec
;

: test2
    payload ['] check_if_0xFF vec
;