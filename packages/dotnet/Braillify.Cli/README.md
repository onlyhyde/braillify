# Braillify

한국어 점자 변환 CLI 도구
Korean Braille Conversion CLI Tool

## 설치 (Installation)

```bash
# 글로벌 설치 / Global installation
dotnet tool install -g Braillify

# 설치 없이 실행 (.NET 10+) / Run without installation (.NET 10+)
dnx braillify "안녕하세요"
```

## 사용법 (Usage)

```bash
# 텍스트 변환 / Convert text
braillify "안녕하세요"
# 출력: ⠣⠒⠉⠻⠚⠠⠝⠬

# 파이프 입력 / Piped input
echo "안녕하세요" | braillify

# REPL 모드 (인자 없이 실행) / REPL mode (run without arguments)
braillify
> 안녕하세요
⠣⠒⠉⠻⠚⠠⠝⠬
> 한글 점자
⠚⠒⠈⠮⠀⠨⠎⠢⠨
> (Ctrl+C로 종료 / Exit with Ctrl+C)
```

## 옵션 (Options)

| 옵션 | 설명 |
|------|------|
| `-h`, `--help` | 도움말 출력 / Print help |
| `--version` | 버전 출력 / Print version |

## 라이선스 (License)

Apache-2.0

## 관련 링크 (Links)

- [Braillify.Core 라이브러리 (NuGet)](https://www.nuget.org/packages/Braillify.Core)
- [GitHub](https://github.com/dev-five-git/braillify)
- [홈페이지 / Website](https://braillify.kr)
