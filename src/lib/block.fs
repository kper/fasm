: remove-nth ( n -- )
  \ Remove an element from stack at the position n (counted from the last element and zero index)
    { n }
    depth
    { depth-size }
    depth-size n - { length }
    length allocate throw
    { base-addr }
    length 1 - { start-index } \ because 0 index
    base-addr
    depth-size start-index ?do
      dup
      >r
      !
      r>
      1 cells +
    loop

    1 cells -
    1 cells -

    depth-size start-index 1 + ?do
      dup
      >r
      @
      r>
      1 cells -
    loop

    drop

    base-addr free throw
;