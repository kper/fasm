#!/usr/bin/env bash

cd wat
for d in *.wat ; do
    echo "$d"
    wat2wasm $d && mv ${d%%.*}.wasm ../wasm/${d%%.*}.wasm
done
