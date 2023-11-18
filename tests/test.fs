: test-equal { w1 w2 -- }
  w1 w2 = if 
    s" OK " type 
  else 
    s" FAIL expected " type
    w2 . 
    s" but got " type 
    w1 .
  endif 
  cr
;