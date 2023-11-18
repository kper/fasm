run:
	gforth src/compiler.fs

test:
	gforth src/leb128.fs tests/leb128.test.fs
	gforth src/magic_bytes.fs tests/magic_bytes.test.fs
