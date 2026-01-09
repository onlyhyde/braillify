# Braillify .NET

Rust 기반 크로스플랫폼 한국어 점역 라이브러리 .NET 바인딩
Cross-platform Korean Braille Library .NET Binding

## 패키지 (Packages)

| 패키지 | 설명 | NuGet |
|--------|------|-------|
| `Braillify.Core` | 라이브러리 / Library | [![NuGet](https://img.shields.io/nuget/v/Braillify.Core)](https://www.nuget.org/packages/Braillify.Core) |
| `Braillify` | CLI 도구 / CLI Tool | [![NuGet](https://img.shields.io/nuget/v/Braillify)](https://www.nuget.org/packages/Braillify) |

## 라이브러리 설치 (Library Installation)

```bash
dotnet add package Braillify.Core
```

## 라이브러리 사용법 (Library Usage)

```csharp
using Braillify;

// 텍스트를 점자 유니코드로 변환
// Convert text to braille unicode
string braille = Braillify.EncodeToUnicode("안녕하세요");
// 결과: "⠣⠒⠉⠻⠚⠠⠝⠬"

// 텍스트를 점자 바이트 배열로 변환
// Convert text to braille byte array
byte[] bytes = Braillify.Encode("안녕하세요");

// 텍스트를 점자 폰트 문자열로 변환
// Convert text to braille font string
string font = Braillify.EncodeToBrailleFont("안녕하세요");
```

## CLI 설치 (CLI Installation)

```bash
# 글로벌 설치 / Global installation
dotnet tool install -g Braillify

# 설치 없이 실행 (.NET 10+) / Run without installation (.NET 10+)
dnx braillify "안녕하세요"
```

## CLI 사용법 (CLI Usage)

```bash
# 텍스트 변환 / Convert text
braillify "안녕하세요"
# 출력: ⠣⠒⠉⠻⠚⠠⠝⠬

# 파이프 입력 / Piped input
echo "안녕하세요" | braillify

# REPL 모드 / REPL mode
braillify
```

## 지원 플랫폼 (Supported Platforms)

- Windows (x64, x86, arm64)
- Linux (x64, arm64)
- macOS (x64, arm64)

## 지원 .NET 버전 (Supported .NET Versions)

- .NET Standard 2.0
- .NET Core 3.1
- .NET 5.0, 6.0, 7.0, 8.0, 9.0, 10.0

## 라이선스 (License)

Apache-2.0

## 링크 (Links)

- [GitHub](https://github.com/dev-five-git/braillify)
- [홈페이지 / Website](https://braillify.kr)
