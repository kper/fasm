require leb128.fs

0 Value fd-in

s" ./wasm/simple.wasm" r/o bin open-file throw Value fd-in

: read_bytes ( addr n -- )
    fd-in read-file throw drop ;

: read_bytes_from_mem_into_stack ( addr u1 -- [u2])
    { addr max }
    max 0 DO addr i + c@ LOOP 
;

: consume_leb128_u { addr -- addr2 value }
    addr LEB128->u ( now, we have the next addr on the stack and the value )
    { next-addr val }
    next-addr addr -
    { consumed-bytes }
    fd-in file-position throw
    { fpos }
    fpos consumed-bytes -
    fd-in reposition-file throw
    next-addr val
;