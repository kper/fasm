(module
  (type (;0;) (func))
  (type (;1;) (func (result i32)))
  (func $__wasm_call_ctors (type 0))
  (func $fib (type 1) (result i32)
    (local i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32 i32)
    global.get $__stack_pointer
    local.set 0
    i32.const 16
    local.set 1
    local.get 0
    local.get 1
    i32.sub
    local.set 2
    i32.const 10
    local.set 3
    local.get 2
    local.get 3
    i32.store offset=12
    i32.const 0
    local.set 4
    local.get 2
    local.get 4
    i32.store offset=8
    i32.const 1
    local.set 5
    local.get 2
    local.get 5
    i32.store offset=4
    block  ;; label = @1
      loop  ;; label = @2
        local.get 2
        i32.load offset=12
        local.set 6
        i32.const 0
        local.set 7
        local.get 6
        local.set 8
        local.get 7
        local.set 9
        local.get 8
        local.get 9
        i32.gt_s
        local.set 10
        i32.const 1
        local.set 11
        local.get 10
        local.get 11
        i32.and
        local.set 12
        local.get 12
        i32.eqz
        br_if 1 (;@1;)
        local.get 2
        i32.load offset=8
        local.set 13
        local.get 2
        i32.load offset=4
        local.set 14
        local.get 13
        local.get 14
        i32.add
        local.set 15
        local.get 2
        local.get 15
        i32.store
        local.get 2
        i32.load offset=4
        local.set 16
        local.get 2
        local.get 16
        i32.store offset=8
        local.get 2
        i32.load
        local.set 17
        local.get 2
        local.get 17
        i32.store offset=4
        local.get 2
        i32.load offset=12
        local.set 18
        i32.const 1
        local.set 19
        local.get 18
        local.get 19
        i32.sub
        local.set 20
        local.get 2
        local.get 20
        i32.store offset=12
        br 0 (;@2;)
      end
    end
    local.get 2
    i32.load
    local.set 21
    local.get 21
    return)
  (memory (;0;) 2)
  (global $__stack_pointer (mut i32) (i32.const 66560))
  (global (;1;) i32 (i32.const 1024))
  (global (;2;) i32 (i32.const 1024))
  (global (;3;) i32 (i32.const 1024))
  (global (;4;) i32 (i32.const 66560))
  (global (;5;) i32 (i32.const 0))
  (global (;6;) i32 (i32.const 1))
  (export "memory" (memory 0))
  (export "__wasm_call_ctors" (func $__wasm_call_ctors))
  (export "fib" (func $fib))
  (export "__dso_handle" (global 1))
  (export "__data_end" (global 2))
  (export "__global_base" (global 3))
  (export "__heap_base" (global 4))
  (export "__memory_base" (global 5))
  (export "__table_base" (global 6)))
