require wasm-runtime.fs

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

\ Single block without br.
: test1
  s" Test 1 .. " type
  0
  0 wasm-block
    1 +
  wasm-end
  2 +
  3 test-equal
;
test1

: test1-1
  s" Test 1-1 .. " type
  42
  0 wasm-block
    666
  wasm-end
  \ TODO: Stack cleanup?
  666 test-equal
  42 test-equal
;
test1-1

: test1-2
  s" Test 1-2 .. " type
  42
  1 wasm-block
    666
  wasm-end
  \ TODO: Stack cleanup?
  666 test-equal
  42 test-equal
;
test1-2

\ Single loop without br.
: test2
  s" Test 2 .. " type
  0
  0 wasm-loop
    1 +
  wasm-end
  2 +
  3 test-equal
;
test2

: test2-1
  s" Test 2-1 .. " type
  42
  0 wasm-loop
    666
  wasm-end
  \ TODO: Stack cleanup?
  666 test-equal
  42 test-equal
;
test2-1

: test2-2
  s" Test 1-2 .. " type
  42
  1 wasm-loop
    666
  wasm-end
  \ TODO: Stack cleanup?
  666 test-equal
  42 test-equal
;
test2-2

\ Two blocks without br.
: test3
  s" Test 3 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
    wasm-end
    4 +
  wasm-end
  8 +
  15 test-equal
;
test3

\ Two loops without br.
: test4
  s" Test 4 .. " type
  0
  0 wasm-loop
    1 +
    0 wasm-loop
      2 +
    wasm-end
    4 +
  wasm-end
  8 +
  15 test-equal
;
test4

\ Single block with single br.
: test5
  s" Test 5 .. " type
  0
  0 wasm-block
    1 +
    0 [ 0 ] wasm-br
    2 +
  wasm-end
  4 +
  5 test-equal
;
test5

: test5-1
  s" Test 5-1 .. " type
  42
  0 wasm-block
    0
    1 +
    0 [ 0 ] wasm-br
    2 +
  wasm-end
  42 test-equal
;
test5-1

\ Single block with single return argument without br.
: test6
  s" Test 6 .. " type
  666
  1 wasm-block
    4 +
    42
    0 [ 0 ] wasm-br
  wasm-end
  42 test-equal
;
test6

\ Single loop with single br.
\ : test6
\   s" Test 6 .. " type
\   0
\   0 wasm-loop
\     1 +
\     0 [ 0 ] wasm-br
\     2 +
\   wasm-end
\   4 +
\   5 test-equal
\ ;
\ test6

\ : main
\   s" Start Program " type cr
\   0 wasm-block
\     s" Start Block 0 " type cr
\     0 wasm-block
\       s" Start Block 1 " type cr
\       [ 0 ] wasm-br
\       s" End Block 1 " type cr
\     wasm-end
\     s" End Block 0 " type cr
\   wasm-end
\   s" End Program " type cr  
\ ;

\ : main
\   0 wasm-loop
\     40
\     s" outer loop " type cr
\     0 wasm-loop
\       s" inner loop " type cr
\       41
\       42
\       0 [ 0 ] wasm-br
\       s" inner loop - !!! " type cr
\     wasm-end
\     s" outer-loop - !!! " type cr
\   wasm-end
\   s" but this we should see " type cr  
\ ;

\ main

bye