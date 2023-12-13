0 Value fd-out
s" ./output.fs" r/w create-file throw to fd-out

: u-to-s ( u -- c-addr u ) 0 <# #s #> ;

: compile-file ( addr u -- )
  fd-out write-file throw
;

: compile-cr ( -- ) 
  s"  " fd-out write-line throw
;

: str->out ( str -- )
  \g Writes a string without an additional 
  \g newline character to output.
  fd-out write-file throw
;

: ln->out ( str -- )
  \g Writes a string with an additional
  \g newline character to output.
  fd-out write-line throw
;

: num->out ( u -- )
  \g Writes a number to output.
  u-to-s str->out
  \ TODO: additional space required?
  s"  "  str->out
;

: cr->out ( -- ) 
  \g Writes a newline character to output.
  s"  " ln->out
;