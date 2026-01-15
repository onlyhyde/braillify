# Braillify

한국어 점자 변환 CLI 도구

## 설치

```bash
# 글로벌 설치
dotnet tool install -g Braillify

# 설치 없이 실행 (.NET 10+)
dnx braillify "안녕하세요"
```

## 사용법

```bash
# 텍스트 변환
braillify "안녕하세요"
# 출력: ⠣⠒⠉⠻⠚⠠⠝⠬

# 파이프 입력
echo "안녕하세요" | braillify

# REPL 모드 (인자 없이 실행)
braillify
> 안녕하세요
⠣⠒⠉⠻⠚⠠⠝⠬
> 한글 점자
⠚⠒⠈⠮⠀⠨⠎⠢⠨
> (Ctrl+C로 종료)
```

## 옵션

| 옵션           | 설명        |
| -------------- | ----------- |
| `-h`, `--help` | 도움말 출력 |
| `--version`    | 버전 출력   |

## 라이선스

Apache-2.0

## 관련 링크

- [Braillify.Net 라이브러리 (NuGet)](https://www.nuget.org/packages/Braillify.Net)
- [GitHub](https://github.com/dev-five-git/braillify)
- [홈페이지](https://braillify.kr)
