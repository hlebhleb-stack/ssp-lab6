using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace SSP_Lab6;

public static class Task2
{
    public static async Task Run()
    {
        Console.WriteLine("=== Задание 2: Базовый MQTT-клиент ===\n");

        // Запускаем локальный брокер на порту 1883
        var serverOptions = new MqttServerOptionsBuilder()
            .WithDefaultEndpointPort(1883)
            .Build();

        var factory = new MqttFactory();
        var server = factory.CreateMqttServer();
        await server.StartAsync(serverOptions);
        Console.WriteLine("Локальный брокер запущен на порту 1883");

        MqttClient client = (MqttClient)factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithClientId("TestClient_" + Guid.NewGuid().ToString()[..6])
            .WithCleanSession()
            .Build();

        client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async args =>
        {
            Console.WriteLine("Подключено к брокеру");
            await client.SubscribeAsync("test/topic", MqttQualityOfServiceLevel.AtLeastOnce);
            Console.WriteLine("Подписались на test/topic\n");
        });

        client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(args =>
        {
            Console.WriteLine("Отключено от брокера");
        });

        client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(args =>
        {
            string msg = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
            Console.WriteLine($"Получено сообщение: {msg}");
        });

        await client.ConnectAsync(options);

        Console.WriteLine("Отправляем 3 тестовых сообщения...\n");
        for (int i = 1; i <= 3; i++)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("test/topic")
                .WithPayload($"Сообщение #{i} от MQTTnet 3.x — {DateTime.Now:HH:mm:ss}")
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await client.PublishAsync(message);
            Console.WriteLine($"Отправлено сообщение #{i}");
            await Task.Delay(1000);
        }

        Console.WriteLine("\nНажмите Enter для выхода...");
        Console.ReadLine();

        await client.DisconnectAsync();
        await server.StopAsync();
        Console.WriteLine("Брокер остановлен");
    }
}
