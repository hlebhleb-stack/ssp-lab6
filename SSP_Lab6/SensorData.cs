namespace SSP_Lab6;

public class SensorData
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double Lighting { get; set; }
    public DateTime Timestamp { get; set; }
    public bool DoorOpen { get; set; }
    public bool AcOn { get; set; }
    public bool HeaterOn { get; set; }
    public int PeopleCount { get; set; }

    public override string ToString()
    {
        return $"{Timestamp:HH:mm:ss} | Темп: {Temperature:F1}°C | Влажность: {Humidity:F1}% | Освещение: {Lighting:F0} лк | Дверь: {(DoorOpen ? "откр" : "закр")} | Люди: {PeopleCount}";
    }
}
