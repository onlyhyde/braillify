// Braillify CLI - 한국어 점자 변환 CLI 도구
// Braillify CLI - Korean Braille Conversion CLI Tool

using System.CommandLine;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

// 입력 텍스트 인자 정의
// Define input text argument
var inputArgument = new Argument<string?>("input")
{
    Description = "변환할 텍스트. 없으면 REPL 모드로 진입합니다. / Text to convert. If omitted, enters REPL mode.",
    Arity = ArgumentArity.ZeroOrOne
};

// 루트 명령 정의
// Define root command
var rootCommand = new RootCommand("한국어 점자 변환 CLI / Korean Braille Conversion CLI")
{
    inputArgument
};

rootCommand.SetAction(parseResult =>
{
    var input = parseResult.GetValue(inputArgument);

    // stdin 파이프 입력 확인
    // Check for piped input from stdin
    if (input is null && Console.IsInputRedirected)
    {
        input = Console.In.ReadToEnd().TrimEnd('\r', '\n');
    }

    if (input is not null)
    {
        // One-shot 모드
        // One-shot mode
        RunOneShot(input);
    }
    else
    {
        // REPL 모드
        // REPL mode
        RunRepl();
    }
});

return rootCommand.Parse(args).Invoke();

static void RunOneShot(string text)
{
    try
    {
        var result = Braillify.Braillify.EncodeToUnicode(text);
        Console.Write(result);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"점자 변환 실패: {ex.Message}");
        Console.Error.WriteLine($"Braille conversion failed: {ex.Message}");
        Environment.ExitCode = 1;
    }
}

static void RunRepl()
{
    Console.WriteLine("braillify REPL - 입력을 점자로 변환합니다. 종료: Ctrl+C or Ctrl+D");
    Console.WriteLine("braillify REPL - Converts input to braille. Exit: Ctrl+C or Ctrl+D");

    while (true)
    {
        Console.Write("> ");

        string? line;
        try
        {
            line = Console.ReadLine();
        }
        catch (Exception)
        {
            break;
        }

        // Ctrl+D (EOF) 또는 null
        // Ctrl+D (EOF) or null
        if (line is null)
        {
            Console.WriteLine("종료합니다. / Exiting.");
            break;
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            continue;
        }

        try
        {
            var result = Braillify.Braillify.EncodeToUnicode(line);
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
