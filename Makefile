run:
	gforth src/compiler.fs

test:
	gforth src/leb128.fs src/leb128.test.fs
