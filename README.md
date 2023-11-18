- [waforth](https://github.com/remko/waforth)


```forth
\ TODO: Requires postpone or immediate magic or macro magic else 
\ TODO: the returns tack contains the return address from this function call.
\ Increments the top element on the return stack.
: r+ ( -- ) r> 1+ >r ;     
```
Maybe useful:
- [Structs](https://www.complang.tuwien.ac.at/forth/gforth/Docs-html/Structures.html#Structures)
- [OOP](https://www.complang.tuwien.ac.at/forth/gforth/Docs-html/Object_002doriented-Forth.html#Object_002doriented-Forth)