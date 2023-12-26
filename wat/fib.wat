(module
  (func $fib (result i32)
    (local i32 i32 i32)
    i32.const 10
    i32.const 0
    i32.const 1

    local.set 2
    local.set 1
    local.set 0

    (loop 
      local.get 1
      local.get 2
      i32.add
      local.get 2
      local.set 1
      local.set 2

      local.get 0
      i32.const 1
      i32.sub
      local.tee 0
      i32.const 0      
      i32.gt_s  
      br_if 0
    )
    local.get 2
    return))
