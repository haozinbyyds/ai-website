using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

/// <summary>
/// C# 注释翻译工具 - 将英文注释翻译成中文
/// </summary>
public class CSharpCommentTranslator
{
    private readonly HttpClient _httpClient;
    private const string GoogleTranslateUrl = "https://translate.googleapis.com/translate_a/element.js?cb=googleTranslateElementInit";
    
    public CSharpCommentTranslator()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// 翻译单个 C# 文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="outputPath">输出文件路径（可选）</param>
    public async Task TranslateFile(string filePath, string outputPath = null)
    {
        // 检查文件是否存在
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"文件未找到: {filePath}");
        }

        // 读取文件内容
        string content = File.ReadAllText(filePath);

        // 翻译内容
        string translatedContent = await TranslateContent(content);

        // 确定输出路径
        string finalOutputPath = outputPath ?? Path.ChangeExtension(filePath, ".translated.cs");

        // 写入文件
        File.WriteAllText(finalOutputPath, translatedContent);
        Console.WriteLine($"翻译完成！输出文件: {finalOutputPath}");
    }

    /// <summary>
    /// 翻译文件内容
    /// </summary>
    private async Task<string> TranslateContent(string content)
    {
        // 单行注释模式: //
        string singleLinePattern = @"//\s*(.+)$";
        
        // 多行注释模式: /* ... */
        string multiLinePattern = @"/\*\s*([\s\S]*?)\s*\*/";

        // 处理单行注释
        content = Regex.Replace(content, singleLinePattern, async match =>
        {
            string commentText = match.Groups[1].Value.Trim();
            string translated = await TranslateText(commentText);
            return $"// {translated}";
        }, RegexOptions.Multiline);

        // 处理多行注释
        content = Regex.Replace(content, multiLinePattern, async match =>
        {
            string commentText = match.Groups[1].Value.Trim();
            string translated = await TranslateText(commentText);
            return $"/* {translated} */";
        });

        return content;
    }

    /// <summary>
    /// 翻译文本（使用 Google Translate API）
    /// </summary>
    private async Task<string> TranslateText(string text)
    {
        try
        {
            // 检测是否已是中文
            if (IsChineseSentence(text))
            {
                return text;
            }

            // 调用翻译 API
            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=zh-CN&dt=t&q={Uri.EscapeDataString(text)}";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            
            // 解析响应（���单的 JSON 解析）
            var translatedText = ExtractTranslation(responseContent);
            
            return translatedText ?? text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"翻译错误: {ex.Message}，原文: {text}");
            return text;
        }
    }

    /// <summary>
    /// 检测是否为中文句子
    /// </summary>
    private bool IsChineseSentence(string text)
    {
        return Regex.IsMatch(text, @"[\u4e00-\u9fa5]+");
    }

    /// <summary>
    /// 提取翻译结果
    /// </summary>
    private string ExtractTranslation(string jsonResponse)
    {
        try
        {
            // Google Translate API 返回格式: [[[translated_text, original_text, ...],...]]
            var match = Regex.Match(jsonResponse, @"\[\[\[""([^""]+)""");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }
        catch { }
        
        return null;
    }

    /// <summary>
    /// 批量翻译文件夹中的所有 C# 文件
    /// </summary>
    public async Task TranslateDirectory(string directoryPath, string outputDirectoryPath = null)
    {
        // 检查目录是否存在
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"目录未找到: {directoryPath}");
        }

        // 确定输出目录
        string finalOutputDir = outputDirectoryPath ?? Path.Combine(directoryPath, "translated");
        Directory.CreateDirectory(finalOutputDir);

        // 获取所有 C# 文件
        var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

        Console.WriteLine($"找到 {csFiles.Length} 个 C# 文件");

        // 批量翻译
        foreach (var file in csFiles)
        {
            try
            {
                Console.WriteLine($"正在翻译: {file}");
                
                string relativePath = Path.GetRelativePath(directoryPath, file);
                string outputPath = Path.Combine(finalOutputDir, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                await TranslateFile(file, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"翻译失败 {file}: {ex.Message}");
            }
        }

        Console.WriteLine("批量翻���完成！");
    }
}

/// <summary>
/// 使用示例
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        var translator = new CSharpCommentTranslator();

        if (args.Length == 0)
        {
            Console.WriteLine("使用方法:");
            Console.WriteLine("  翻译单个文件: dotnet run <input_file.cs> [output_file.cs]");
            Console.WriteLine("  翻译文件夹: dotnet run -d <directory_path> [output_directory]");
            return;
        }

        try
        {
            if (args[0] == "-d" || args[0] == "--directory")
            {
                // 批量翻译模式
                string dirPath = args.Length > 1 ? args[1] : ".";
                string outputDir = args.Length > 2 ? args[2] : null;
                await translator.TranslateDirectory(dirPath, outputDir);
            }
            else
            {
                // 单文件翻译模式
                string inputFile = args[0];
                string outputFile = args.Length > 1 ? args[1] : null;
                await translator.TranslateFile(inputFile, outputFile);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
