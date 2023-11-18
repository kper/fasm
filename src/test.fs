: test-equal { w1 w2 -- }
  w1 w2 = if 
    s" OK " type 
  else 
    s" FAIL " type
    w1 . 
    s" is not equal to " type 
    w2 .
  endif 
  cr
;