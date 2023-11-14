0 Value fd-in

s" ./simple.wasm" r/o bin open-file throw Value fd-in

: read_bytes ( addr n -- )
    fd-in read-file throw drop .s ;

: read_bytes_from_mem_into_stack ( addr u1 -- [u2])
    { addr max }
    max 0 DO addr i + c@ LOOP 
;