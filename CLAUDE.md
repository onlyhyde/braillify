# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Braillify is a Korean Braille transliteration library based on the 2024 Korean Braille Standard. The core library is written in Rust and provides bindings for Node.js (via WebAssembly), Python (via PyO3), and .NET (via FFI/P/Invoke).

## Build & Development Commands

```bash
# Install dependencies (includes uv sync, wasm-pack, maturin)
bun install

# Build all packages (Rust, Node.js WASM, Python)
bun run build

# Build landing page (runs build + test_by_testcase + Next.js build)
bun run build:landing

# Run all tests (Rust with tarpaulin, Vitest, Python pytest)
bun run test

# Lint
bun run lint
bun run lint:fix
```

### Individual Package Commands

```bash
# Node.js WASM package
cd packages/node && wasm-pack build --target bundler --out-dir ./pkg --out-name index

# Python package
cd packages/python && maturin build --release --out dist

# Rust tests only
cargo test

# Rust tests with coverage
cargo tarpaulin --out xml --out stdout

# Run specific Rust test (e.g., test_by_testcase reads all CSV files in test_cases/)
cargo test test_by_testcase

# Node.js tests
vitest test --run

# Python tests
cd py-test && uv run pytest __init__.py

# .NET native library
bun run build:dotnet-native
# or: cargo build --release -p dotnet

# .NET package
bun run build:dotnet
# or: cd packages/dotnet && dotnet build -c Release

# .NET tests
bun run test:dotnet
# or: cd packages/dotnet && dotnet test
```

## Architecture

```
braillify/
├── libs/braillify/           # Core Rust library
│   └── src/
│       ├── lib.rs            # Main encoder (Encoder struct, encode/encode_to_unicode)
│       ├── cli.rs            # CLI implementation
│       ├── korean_char.rs    # Korean character encoding
│       ├── jauem/            # Korean consonant (자음) handling
│       ├── moeum/            # Korean vowel (모음) handling
│       ├── english.rs        # English character encoding
│       ├── number.rs         # Number encoding
│       ├── symbol_shortcut.rs    # Symbol mappings
│       ├── word_shortcut.rs      # Word abbreviation mappings
│       ├── rule.rs           # Korean braille rules (한글 점자 규정)
│       ├── rule_en.rs        # English braille rules
│       └── fraction.rs       # Fraction handling (LaTeX, Unicode)
├── packages/
│   ├── node/                 # Node.js WASM bindings (wasm-bindgen)
│   │   └── src/lib.rs        # Exposes: encode, translateToUnicode, translateToBrailleFont
│   ├── python/               # Python bindings (PyO3/maturin)
│   │   └── src/lib.rs        # Exposes: encode, translate_to_unicode, translate_to_braille_font, cli
│   └── dotnet/               # .NET bindings (FFI/P/Invoke)
│       ├── src/lib.rs        # C ABI exports for .NET P/Invoke
│       └── Braillify/        # .NET class library (Braillify.Encode, EncodeToUnicode, EncodeToBrailleFont)
├── apps/landing/             # Next.js landing page (braillify.com)
├── test_cases/               # CSV test files for Korean braille rules (rule_1.csv ~ rule_63.csv)
└── py-test/                  # Python test suite
```

## Key Implementation Details

### Encoder State Machine
The `Encoder` struct in `libs/braillify/src/lib.rs` maintains state for:
- `is_english`: Whether currently in English/Roman character mode
- `english_indicator`: Whether Korean text requires Roman character markers
- `triple_big_english`: Tracking consecutive uppercase word sequences
- `parenthesis_stack`: Tracking parenthesis context for symbol rendering

### Korean Braille Rules
Test cases in `test_cases/` correspond to 2024 Korean Braille Standard rules. The `test_by_testcase` test validates all rules by reading CSV files. Each CSV has format: `input,internal_code,expected_code,expected_unicode`.

### Multi-platform Support
- **Rust**: Native library with CLI (`braillify` binary requires `cli` feature)
- **Node.js**: WebAssembly via wasm-pack, published as `braillify` npm package
- **Python**: Native extension via PyO3/maturin, published as `braillify` on PyPI
- **.NET**: Native library via FFI/P/Invoke, published as `Braillify` NuGet package

## Testing

Run `cargo test test_by_testcase` to validate against all braille rules. This test reads all CSV files in `test_cases/` and outputs detailed failure information with colored diffs.
