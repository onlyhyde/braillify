# CLAUDE.md

이 파일은 Claude Code (claude.ai/code)가 이 저장소의 코드를 작업할 때 참고하는 가이드입니다.

## 프로젝트 개요

Braillify는 2024 한글 점자 규정을 기반으로 한 한국어 점역 라이브러리입니다. 핵심 라이브러리는 Rust로 작성되었으며, Node.js (WebAssembly), Python (PyO3), .NET (FFI/P/Invoke) 바인딩을 제공합니다.

## 빌드 및 개발 명령어

```bash
# 의존성 설치 (uv sync, wasm-pack, maturin 포함)
bun install

# 모든 패키지 빌드 (Rust, Node.js WASM, Python)
bun run build

# 랜딩 페이지 빌드 (build + test_by_testcase + Next.js build)
bun run build:landing

# 모든 테스트 실행 (Rust tarpaulin, Vitest, Python pytest)
bun run test

# 린트
bun run lint
bun run lint:fix
```

### 개별 패키지 명령어

```bash
# Node.js WASM 패키지
cd packages/node && wasm-pack build --target bundler --out-dir ./pkg --out-name index

# Python 패키지
cd packages/python && maturin build --release --out dist

# Rust 테스트
cargo test

# Rust 테스트 (커버리지 포함)
cargo tarpaulin --out xml --out stdout

# 특정 Rust 테스트 실행 (test_by_testcase는 test_cases/ 내 모든 CSV 파일 읽음)
cargo test test_by_testcase

# Node.js 테스트
vitest test --run

# Python 테스트
cd py-test && uv run pytest __init__.py
```

### .NET 패키지 명령어

```bash
# 네이티브 라이브러리 빌드
bun run build:dotnet-native
# 또는: cargo build --release -p dotnet

# .NET 패키지 빌드
bun run build:dotnet
# 또는: cd packages/dotnet && dotnet build -c Release

# .NET 테스트 (모든 프레임워크)
bun run test:dotnet
# 또는: cd packages/dotnet && dotnet test

# 특정 프레임워크 테스트
bun run test:dotnet:net5.0
bun run test:dotnet:net6.0
bun run test:dotnet:net7.0
bun run test:dotnet:net8.0
bun run test:dotnet:net9.0
bun run test:dotnet:net10.0
bun run test:dotnet:net472
```

## 아키텍처

```
braillify/
├── libs/braillify/           # 핵심 Rust 라이브러리
│   └── src/
│       ├── lib.rs            # 메인 인코더 (Encoder 구조체, encode/encode_to_unicode)
│       ├── cli.rs            # CLI 구현
│       ├── korean_char.rs    # 한글 문자 인코딩
│       ├── jauem/            # 자음 처리
│       ├── moeum/            # 모음 처리
│       ├── english.rs        # 영문자 인코딩
│       ├── number.rs         # 숫자 인코딩
│       ├── symbol_shortcut.rs    # 기호 매핑
│       ├── word_shortcut.rs      # 약어 매핑
│       ├── rule.rs           # 한글 점자 규정
│       ├── rule_en.rs        # 영문 점자 규정
│       └── fraction.rs       # 분수 처리 (LaTeX, Unicode)
├── packages/
│   ├── node/                 # Node.js WASM 바인딩 (wasm-bindgen)
│   │   └── src/lib.rs        # 노출: encode, translateToUnicode, translateToBrailleFont
│   ├── python/               # Python 바인딩 (PyO3/maturin)
│   │   └── src/lib.rs        # 노출: encode, translate_to_unicode, translate_to_braille_font, cli
│   └── dotnet/               # .NET 바인딩 (FFI/P/Invoke)
│       ├── Cargo.toml        # Rust FFI 크레이트 설정
│       ├── src/lib.rs        # C ABI 노출 함수 (braillify_encode, braillify_encode_to_unicode 등)
│       ├── BraillifyNet/     # .NET 클래스 라이브러리 (NuGet: BraillifyNet)
│       │   ├── Braillify.cs          # 공개 API (Encode, EncodeToUnicode, EncodeToBrailleFont)
│       │   ├── NativeMethods.cs      # P/Invoke 선언 (버전별 전처리)
│       │   ├── NativeLibraryLoader.cs # 플랫폼별 네이티브 라이브러리 로딩
│       │   └── BraillifyException.cs  # 예외 클래스
│       ├── Braillify/        # CLI 도구 (NuGet: Braillify, 명령어: braillify)
│       │   └── Program.cs            # System.CommandLine 기반 CLI
│       ├── Braillify.Tests/           # xUnit 테스트 (멀티타겟팅)
│       └── Braillify.Tests.NetFramework/ # MSTest (.NET Framework 4.7.2)
├── apps/landing/             # Next.js 랜딩 페이지 (braillify.com)
├── test_cases/               # CSV 테스트 파일 (rule_1.csv ~ rule_63.csv)
└── py-test/                  # Python 테스트 스위트
```

## 핵심 구현 세부사항

### 인코더 상태 머신

`libs/braillify/src/lib.rs`의 `Encoder` 구조체는 다음 상태를 유지합니다:

- `is_english`: 현재 영문/로마자 모드 여부
- `english_indicator`: 한글 텍스트에 로마자 표시자 필요 여부
- `triple_big_english`: 연속 대문자 단어 시퀀스 추적
- `parenthesis_stack`: 기호 렌더링을 위한 괄호 컨텍스트 추적

### 한글 점자 규정

`test_cases/`의 테스트 케이스는 2024 한글 점자 규정에 대응합니다. `test_by_testcase` 테스트는 CSV 파일을 읽어 모든 규정을 검증합니다. CSV 형식: `input,internal_code,expected_code,expected_unicode`.

### 멀티 플랫폼 지원

- **Rust**: CLI 포함 네이티브 라이브러리 (`braillify` 바이너리는 `cli` 피처 필요)
- **Node.js**: wasm-pack을 통한 WebAssembly, `braillify` npm 패키지로 배포
- **Python**: PyO3/maturin을 통한 네이티브 확장, `braillify` PyPI 패키지로 배포
- **.NET**: FFI/P/Invoke를 통한 네이티브 라이브러리
  - `BraillifyNet`: 라이브러리 NuGet 패키지
  - `Braillify`: CLI 도구 NuGet 패키지 (`dnx braillify` 또는 `dotnet tool install -g Braillify`)

## .NET 바인딩 상세

### NuGet 패키지

| 패키지 | 설명 | 타겟 프레임워크 |
|--------|------|----------------|
| `BraillifyNet` | 라이브러리 | netstandard2.0, netcoreapp3.1, net5.0~net10.0 |
| `Braillify` | CLI 도구 | net10.0 |

### 지원 플랫폼

- .NET Standard 2.0
- .NET Core 3.1
- .NET 5.0, 6.0, 7.0, 8.0, 9.0, 10.0
- .NET Framework 4.7.2 (테스트 지원)

### 버전별 구현 차이 (NativeMethods.cs)

| 버전              | P/Invoke 방식               | 포인터 타입    | UTF-8 마샬링           |
| ----------------- | --------------------------- | -------------- | ---------------------- |
| .NET 7+           | LibraryImport (소스 생성기) | nint/nuint     | StringMarshalling.Utf8 |
| .NET 5-6          | DllImport                   | nint/nuint     | LPUTF8Str              |
| .NET Core 3.1     | DllImport                   | IntPtr/UIntPtr | LPUTF8Str              |
| .NET Standard 2.0 | DllImport                   | IntPtr/UIntPtr | 수동 UTF-8 변환        |

### 네이티브 라이브러리 로딩 (NativeLibraryLoader.cs)

- .NET Core 3.1+: `NativeLibrary.SetDllImportResolver` 사용
- `AppContext.BaseDirectory` 사용 (NativeAOT 호환)
- 런타임 식별자(RID) 기반 경로 탐색: `runtimes/{rid}/native/{lib}`

### 테스트 패키지 호환성 (Braillify.Tests.csproj)

| 프레임워크              | Microsoft.NET.Test.Sdk | xunit | xunit.runner.visualstudio |
| ----------------------- | ---------------------- | ----- | ------------------------- |
| .NET Core 3.1, .NET 5.0 | 17.8.0                 | 2.4.2 | 2.4.5                     |
| .NET 6.0, 7.0           | 17.12.0                | 2.6.6 | 2.5.6                     |
| .NET 8.0+               | 18.\*                  | 2.\*  | 3.\*                      |

> **주의**: xunit.runner.visualstudio 3.x는 .NET 8.0 이상에서만 지원됩니다.

### CLI 사용법

```bash
# 글로벌 설치
dotnet tool install -g Braillify

# 설치 없이 실행 (.NET 10+)
dnx braillify "안녕하세요"

# 텍스트 변환
braillify "안녕하세요"
# 출력: ⠣⠒⠉⠻⠚⠠⠝⠬

# 파이프 입력
echo "안녕하세요" | braillify

# REPL 모드 (인자 없이 실행)
braillify
```

## 테스트

`cargo test test_by_testcase`를 실행하여 모든 점자 규정을 검증합니다. 이 테스트는 `test_cases/`의 모든 CSV 파일을 읽고 실패 시 컬러 diff와 함께 상세 정보를 출력합니다.
