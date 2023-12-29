10 0 1 local-stack 2  cells + !
local-stack 1  cells + !
local-stack 0  cells + !
0 wasm-loop
local-stack 1 cells + @
local-stack 2 cells + @
+
local-stack 2 cells + @
local-stack 1  cells + !
local-stack 2  cells + !
local-stack 0 cells + @
1 -
dup local-stack 0 cells + !
0 >
0 [ 0 ] wasm-br-if
wasm-end
local-stack 2 cells + @
