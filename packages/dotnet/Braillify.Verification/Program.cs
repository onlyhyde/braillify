// Braillify NuGet 패키지 검증 프로그램
// Braillify NuGet Package Verification Program

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== Braillify NuGet 패키지 검증 ===");
Console.WriteLine("=== Braillify NuGet Package Verification ===\n");

// 테스트 케이스
// Test cases
var testCases = new (string Input, string ExpectedUnicode)[]
{
    ("안녕하세요", "⠣⠒⠉⠻⠚⠠⠝⠬"),
    ("상상이상의", "⠇⠶⠇⠶⠕⠇⠶⠺"),
    ("1,000", "⠼⠁⠂⠚⠚⠚"),
    ("ATM", "⠠⠠⠁⠞⠍"),
    ("한글 점자", null!), // 예상 결과 없이 테스트 / Test without expected result
};

var passCount = 0;
var failCount = 0;

foreach (var (input, expectedUnicode) in testCases)
{
    try
    {
        Console.WriteLine($"입력 (Input): \"{input}\"");

        // EncodeToUnicode 테스트
        // EncodeToUnicode test
        var unicodeResult = Braillify.Braillify.EncodeToUnicode(input);
        Console.WriteLine($"유니코드 (Unicode): {unicodeResult}");

        // Encode 테스트 (바이트 배열)
        // Encode test (byte array)
        var byteResult = Braillify.Braillify.Encode(input);
        Console.WriteLine($"바이트 배열 길이 (Byte array length): {byteResult.Length}");

        // EncodeToBrailleFont 테스트
        // EncodeToBrailleFont test
        var fontResult = Braillify.Braillify.EncodeToBrailleFont(input);
        Console.WriteLine($"폰트 문자열 (Font string): {fontResult}");

        // 예상 결과와 비교
        // Compare with expected result
        if (expectedUnicode != null)
        {
            if (unicodeResult == expectedUnicode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[PASS] 예상 결과와 일치 / Matches expected result");
                Console.ResetColor();
                passCount++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] 예상: {expectedUnicode}, 실제: {unicodeResult}");
                Console.WriteLine($"[FAIL] Expected: {expectedUnicode}, Actual: {unicodeResult}");
                Console.ResetColor();
                failCount++;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[INFO] 예상 결과 없음 - 출력만 확인 / No expected result - output only");
            Console.ResetColor();
            passCount++;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] 예외 발생: {ex.Message}");
        Console.WriteLine($"[ERROR] Exception: {ex.Message}");
        Console.ResetColor();
        failCount++;
    }

    Console.WriteLine();
}

// 요약 출력
// Summary output
Console.WriteLine("=== 검증 결과 요약 (Verification Summary) ===");
Console.ForegroundColor = passCount > 0 && failCount == 0 ? ConsoleColor.Green : ConsoleColor.Yellow;
Console.WriteLine($"통과 (Passed): {passCount}");
Console.ForegroundColor = failCount > 0 ? ConsoleColor.Red : ConsoleColor.Green;
Console.WriteLine($"실패 (Failed): {failCount}");
Console.ResetColor();

// null 입력 예외 테스트
// Null input exception test
Console.WriteLine("\n=== Null 입력 예외 테스트 (Null Input Exception Test) ===");
try
{
    Braillify.Braillify.EncodeToUnicode(null!);
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("[FAIL] ArgumentNullException이 발생하지 않음 / ArgumentNullException not thrown");
    Console.ResetColor();
}
catch (ArgumentNullException)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("[PASS] ArgumentNullException 정상 발생 / ArgumentNullException thrown as expected");
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"[FAIL] 예상치 못한 예외: {ex.GetType().Name}");
    Console.WriteLine($"[FAIL] Unexpected exception: {ex.GetType().Name}");
    Console.ResetColor();
}

// 최종 결과
// Final result
Console.WriteLine();
if (failCount == 0)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("*** 모든 검증 통과! NuGet 패키지가 정상 동작합니다. ***");
    Console.WriteLine("*** All verifications passed! NuGet package works correctly. ***");
    Console.ResetColor();
    return 0;
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("*** 일부 검증 실패. 확인이 필요합니다. ***");
    Console.WriteLine("*** Some verifications failed. Review needed. ***");
    Console.ResetColor();
    return 1;
}
