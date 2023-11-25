0 Value fd-out
s" ./output.fs" r/w create-file throw to fd-out

: u-to-s ( u -- c-addr u ) 0 <# #s #> ;

: compile-file ( addr u -- )
    fd-out write-file throw
;

: compile-cr ( -- ) 
    s"  " fd-out write-line throw
;