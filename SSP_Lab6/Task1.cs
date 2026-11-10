namespace SSP_Lab6;

public static class Task1
{
    private static readonly string _logFile = "task1_sensor_log.txt";

    public static async Task Run()
    {
        Console.WriteLine("=== Задание 1: Датчик → файл ===\n");

        var sensor = new EnvironmentAwareSensor();

        Console.WriteLine($"Запись данных в файл: {_logFile}");
        Console.WriteLine("Нажмите Ctrl+C для остановки\n");

        int cycle = 0;
        while (true)
        {
            await Task.Delay(2000);
            var data = sensor.ReadData();

            string line = data.ToString();
            Console.WriteLine($"[{cycle++}] {line}");

            await File.AppendAllTextAsync(_logFile, line + Environment.NewLine);
            Console.WriteLine($"    Сохранено в {_logFile}");
        }
    }
}
