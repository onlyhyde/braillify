# CORE LIBRARY (libs/braillify)

Korean Braille encoding engine implementing 2024 Korean Braille Standard.

## STRUCTURE

```
src/
├── lib.rs              # Main Encoder struct, encode() entry point
├── cli.rs              # CLI: REPL + one-shot mode (feature-gated)
├── main.rs             # Binary entry point
├── korean_char.rs      # Full Korean syllable encoding
├── korean_part.rs      # Standalone jamo (consonant/vowel) encoding
├── jauem/              # Consonant handling
│   ├── choseong.rs     # Initial consonants
│   └── jongseong.rs    # Final consonants
├── moeum/              # Vowel handling
│   └── jungsong.rs     # Medial vowels
├── rule.rs             # Korean Braille rules (11, 12, etc.)
├── rule_en.rs          # English abbreviation rules (10-4, 10-6)
├── english.rs          # English letter encoding
├── english_logic.rs    # English context detection
├── number.rs           # Number encoding
├── fraction.rs         # Fraction handling (Unicode + LaTeX)
├── *_shortcut.rs       # PHF static lookup tables
├── unicode.rs          # Internal code to Unicode Braille
├── split.rs            # Korean jamo decomposition
├── char_struct.rs      # CharType enum (Korean/English/Number/Symbol)
└── utils.rs            # Helper functions
```

## KEY TYPES

| Type         | Location         | Purpose                                                 |
| ------------ | ---------------- | ------------------------------------------------------- |
| `Encoder`    | `lib.rs`         | Stateful encoder tracking English mode, uppercase state |
| `CharType`   | `char_struct.rs` | Input character classification                          |
| `KoreanChar` | `korean_char.rs` | Decomposed Korean syllable (cho/jung/jong)              |

## ENTRY POINTS

| Function                  | Location     | Usage                             |
| ------------------------- | ------------ | --------------------------------- |
| `encode(text)`            | `lib.rs:634` | Returns `Result<Vec<u8>, String>` |
| `encode_to_unicode(text)` | `lib.rs:648` | Returns Braille Unicode string    |
| `run_cli(args)`           | `cli.rs:16`  | CLI entry (feature: cli)          |

## RULE IMPLEMENTATION

Korean comments reference rule numbers from 2024 Korean Braille Standard:

- `제8항` - Standalone jamo
- `제11항` - Vowel + 예 separator
- `제14항` - 나/다/마... + vowel (no abbreviation)
- `제28항` - Uppercase handling
- `제31항` - Roman letter indicators
- `제40항` - Number prefix
- `제43항` - Numbers with punctuation
- `제44항` - Number + Korean spacing

## CONVENTIONS

- PHF macros (`phf_map!`) for all static lookup tables
- Error handling via `Result<T, String>` - propagate, never suppress
- Feature flags: `cli` (default), `wasm`
- Tests inline with `#[cfg(test)]` in each module

## ANTI-PATTERNS

- **Never use `unwrap()` on user input** - return `Err(String)`
- **Never hardcode Braille dots** - use constants or PHF tables
- **Never modify shortcut tables** without updating test CSVs

## TESTING

```bash
# Run all tests with coverage
cargo tarpaulin -p braillify

# Run specific test
cargo test test_encode

# Generate test_status.json for landing page
cargo test test_by_testcase
```

Tests read from `../../test_cases/*.csv` - format: `input,internal_repr,expected,unicode`
