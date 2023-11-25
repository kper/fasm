(module
  (func $temp (result i32 i32)
    (block $block1 (result i32 i32)
      (block $block2 (result i32)
        (block $block3 (result i32)
          i32.const 10
          br $block2))
      i32.const 20
    ) 
)
(export "temp" (func $temp))
)
