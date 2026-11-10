using System.Diagnostics;

namespace SSP_Lab6;

public static class Task4
{
    public static async Task Run()
    {
        Console.WriteLine("=== Задание 4: Сценарий нагрева комнаты ===\n");
        Console.WriteLine("Старт: холодная комната, 0°C");
        Console.WriteLine("Писк: когда температура достигнет 10°C");
        Console.WriteLine("Команды: door, ac, heater, add, remove, light, status, exit\n");

        var sensor = new EnvironmentAwareSensor();
        sensor.SetBaseTemperature(0.0);

        sensor.CloseDoor();
        for (int i = 0; i < 15; i++)
            sensor.AddPerson();
        sensor.TurnOnHeater();

        Console.WriteLine("Дверь закрыта, добавлено 15 человек, обогреватель включён\n");

        _ = Task.Run(() => ProcessCommands(sensor));

        bool beeped = false;
        int cycle = 0;

        while (true)
        {
            await Task.Delay(2000);
            var data = sensor.ReadData();

            Console.WriteLine($"\n[{cycle++}] {data}");

            if (!beeped && data.Temperature >= 10.0)
            {
                beeped = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">>> Температура достигла 10°C! <<<");
                Console.ResetColor();

                if (OperatingSystem.IsWindows())
                    Console.Beep(1000, 500);
                else if (OperatingSystem.IsMacOS())
                {
                    var p = Process.Start(new ProcessStartInfo("afplay", "/System/Library/Sounds/Ping.aiff") { UseShellExecute = false });
                    p?.WaitForExit();
                }
            }
        }
    }

    private static async Task ProcessCommands(EnvironmentAwareSensor sensor)
    {
        await Task.Yield();
        while (true)
        {
            var input = Console.ReadLine()?.ToLower().Trim();
            switch (input)
            {
                case "door":
                    Console.Write("open/close: ");
                    if (Console.ReadLine()?.ToLower().Trim() == "open") sensor.OpenDoor();
                    else sensor.CloseDoor();
                    break;
                case "ac":
                    Console.Write("on/off: ");
                    if (Console.ReadLine()?.ToLower().Trim() == "on") sensor.TurnOnAC();
                    else sensor.TurnOffAC();
                    break;
                case "heater":
                    Console.Write("on/off: ");
                    if (Console.ReadLine()?.ToLower().Trim() == "on") sensor.TurnOnHeater();
                    else sensor.TurnOffHeater();
                    break;
                case "add":
                    sensor.AddPerson();
                    Console.WriteLine("Человек добавлен");
                    break;
                case "remove":
                    sensor.RemovePerson();
                    Console.WriteLine("Человек удалён");
                    break;
                case "light":
                    Console.Write("Уровень освещения (лк): ");
                    if (double.TryParse(Console.ReadLine(), out double lux))
                        sensor.SetLighting(lux);
                    break;
                case "status":
                    Console.WriteLine($"\n{sensor.ReadData()}\n");
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
