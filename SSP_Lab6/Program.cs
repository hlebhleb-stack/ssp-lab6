using Avalonia;

namespace SSP_Lab6;

class Program
{
    static async Task Main(string[] args)
    {
        int task = args.Length > 0 && int.TryParse(args[0], out int n) ? n : 0;

        switch (task)
        {
            case 1:
                await Task1.Run();
                break;
            case 2:
                await Task2.Run();
                break;
            case 3:
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
                break;
            case 4:
                await Task4.Run();
                break;
            default:
                Console.WriteLine("Укажите номер задания: dotnet run -- 1, 2 или 3");
                Console.WriteLine("  1 - Датчик → файл");
                Console.WriteLine("  2 - Базовый MQTT-клиент");
                Console.WriteLine("  3 - Полная интеграция (датчик + MQTT + график)");
                break;
        }
    }

    static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
