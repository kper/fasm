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

\ Multiple blocks with single br.
: test7
  s" Test 7 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      0 [ 0 ] wasm-br
      4 +
    wasm-end
    8 +
  wasm-end
  16 +
  27 test-equal
;
test7

: test7-1
  s" Test 7-1 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      1 [ 1 ] wasm-br
      4 +
    wasm-end
    8 +
  wasm-end
  16 +
  19 test-equal
;
test7-1

: test7-2
  s" Test 7-2 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      0 wasm-block
        4 +
        1 [ 1 ] wasm-br
        8 +
      wasm-end
      16 +
    wasm-end
    32 +
  wasm-end
  39 test-equal
;
test7-2

: test7-3
  s" Test 7-3 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      0 wasm-block
        4 +
        2 [ 2 ] wasm-br
        8 +
      wasm-end
      16 +
    wasm-end
    32 +
  wasm-end
  7 test-equal
;
test7-3

\ Single block with single br-if.
: test8
  s" Test 8 .. " type
  0
  0 wasm-block
    1 +
    0 0 [ 0 ] wasm-br-if
    2 +
  wasm-end
  4 +
  7 test-equal
;
test8

: test8-1
  s" Test 8-1 .. " type
  0
  0 wasm-block
    1 +
    -1 0 [ 0 ] wasm-br-if
    2 +
  wasm-end
  4 +
  5 test-equal
;
test8-1

\ Multiple blocks with single br-if.
: test9
  s" Test 9 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      0 0 [ 0 ] wasm-br-if
      4 +
    wasm-end
    8 +
  wasm-end
  16 +
  31 test-equal
;
test9

: test9-1
  s" Test 9-1 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      1 0 [ 0 ] wasm-br-if
      4 +
    wasm-end
    8 +
  wasm-end
  16 +
  27 test-equal
;
test9-1

: test9-2
  s" Test 9-2 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      0 1 [ 1 ] wasm-br-if
      4 +
    wasm-end
    8 +
  wasm-end
  16 +
  31 test-equal
;
test9-2

: test9-3
  s" Test 9-3 .. " type
  0
  0 wasm-block
    1 +
    0 wasm-block
      2 +
      -1 1 [ 1 ] wasm-br-if
      4 +
    wasm-end
    8 +
  wasm-end
  16 +
  19 test-equal
;
test9-3

\ Single loop with single br-if.
: test10
  s" Test 10 .. " type
  0
  0 wasm-block
    0 wasm-loop
      1 +
      dup 10 < 0 [ 0 ] wasm-br-if
    wasm-end
  wasm-end
  10 test-equal
;
test10

bye