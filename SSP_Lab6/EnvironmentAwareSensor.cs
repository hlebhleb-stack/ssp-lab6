namespace SSP_Lab6;

public class EnvironmentAwareSensor
{
    private readonly Random _random = new Random();
    private double _baseTemperature = 22.0;
    private readonly double _noiseAmplitude = 3.0;
    private bool _doorOpen = false;
    private bool _acOn = false;
    private bool _heaterOn = false;
    private int _peopleCount = 0;
    private double _baseLighting = 300.0;

    public SensorData ReadData()
    {
        double noise = (_random.NextDouble() * 2 - 1) * _noiseAmplitude;
        double temperature = _baseTemperature + noise;
        double humidity = 60 + (temperature - _baseTemperature) * 2 + (_random.NextDouble() * 2 - 1) * 5;
        double lighting = _baseLighting + (_random.NextDouble() * 2 - 1) * 50;

        if (_doorOpen) { temperature += 3.0; lighting += 100; }
        if (_acOn) temperature -= 2.0;
        if (_heaterOn) temperature += 4.0;
        temperature += _peopleCount * 0.5;

        return new SensorData
        {
            Temperature = Math.Round(temperature, 2),
            Humidity = Math.Round(humidity, 1),
            Lighting = Math.Round(lighting, 1),
            Timestamp = DateTime.UtcNow,
            DoorOpen = _doorOpen,
            AcOn = _acOn,
            HeaterOn = _heaterOn,
            PeopleCount = _peopleCount
        };
    }

    public void OpenDoor() => _doorOpen = true;
    public void CloseDoor() => _doorOpen = false;
    public void TurnOnAC() => _acOn = true;
    public void TurnOffAC() => _acOn = false;
    public void TurnOnHeater() => _heaterOn = true;
    public void TurnOffHeater() => _heaterOn = false;
    public void AddPerson() => _peopleCount++;
    public void RemovePerson() { if (_peopleCount > 0) _peopleCount--; }
    public void SetLighting(double lux) => _baseLighting = lux;
    public void SetBaseTemperature(double temp) => _baseTemperature = temp;
}
