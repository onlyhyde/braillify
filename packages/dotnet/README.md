# Braillify

Rust 기반 크로스플랫폼 한국어 점역 라이브러리 .NET 바인딩

## 설치

```bash
dotnet add package Braillify
```

## 사용법

```csharp
using Braillify;

// 텍스트를 점자 유니코드로 변환
string braille = Braillify.EncodeToUnicode("안녕하세요");
// 결과: "⠣⠒⠉⠻⠚⠠⠝⠬"

// 텍스트를 점자 바이트 배열로 변환
byte[] bytes = Braillify.Encode("안녕하세요");

// 텍스트를 점자 폰트 문자열로 변환
string font = Braillify.EncodeToBrailleFont("안녕하세요");
```

## 지원 플랫폼

- Windows (x64, x86, arm64)
- Linux (x64, arm64)
- macOS (x64, arm64)

## 라이선스

Apache-2.0

## 링크

- [GitHub](https://github.com/dev-five-git/braillify)
- [웹사이트](https://braillify.kr)
