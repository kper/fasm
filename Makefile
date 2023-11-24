run:
	gforth src/fasm.fs ./wasm/nested_block.wasm

test:
	gforth src/leb128.fs tests/leb128.test.fs
	gforth src/magic_bytes.fs tests/magic_bytes.test.fs
	gforth src/version_bytes.fs tests/version_bytes.test.fs
	gforth src/compiler.fs tests/compiler.test.fs
	gforth src/compiler.fs src/section.fs tests/section.test.fs
	gforth src/utils.fs tests/utils.test.fs
	gforth src/lib/block.fs tests/lib/block.test.fs
