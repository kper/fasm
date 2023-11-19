require io.fs

: vec {  addr xt } ( addr xt -- )
    \g Executes the xt n times.
    \ n is encoded as leb128 in the addr
    addr consume_leb128_u \ Read the n
    { base-addr n } 
    base-addr n 0 do xt execute loop \ Execute the execution token. It is important that the xt puts the incremented addr on the stack.
;