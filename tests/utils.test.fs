require ../src/io.fs

create payload 03 , 1 , 1 , 1

: sum { sum addr -- }
    sum 1 +
    addr 1 + ( increment it for the next iteration )
;

: test_sum 
    0 payload ['] sum vec
;

test_sum