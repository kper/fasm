- [waforth](https://github.com/remko/waforth)


```forth
\ TODO: Requires postpone or immediate magic or macro magic else 
\ TODO: the returns tack contains the return address from this function call.
\ Increments the top element on the return stack.
: r+ ( -- ) r> 1+ >r ;     
```