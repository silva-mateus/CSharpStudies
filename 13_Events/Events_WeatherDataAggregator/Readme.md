Events - WeatherDataAggregator
In the existing code, we have several classes:

1) WeatherData record struct, which contains nullable Temperature and nullable Humidity

2) WeatherDataAggregator, whose job is to receive notifications from classes that can analyze current weather and store those notifications in the WeatherHistory collection. Each item in this collection is of the WeatherData type.

3) WeatherStation, which can measure the current Temperature.

4) WeatherBaloon, which can measure the current Humidity.

5) WeatherDataEventArgs derived from EventArgs, which carries a WeatherData object.

We want the following code to work correctly after the exercise is implemented:

```csharp
var weatherDataAggregator = new WeatherDataAggregator();
var weatherstation = new Weatherstation();
weatherStation.WeatherMeasured +=
    weatherDataAggregator. GetNotifiedAboutNewData ;
var weatherBaloon new WeatherBaloon();
weatherBaloon.Weathermeasured +=
    weatherDataAggregator.GetNotifiedAboutNewData;

weatherStation.Measure();
weatherBaloon.Measure();
//at this point, weatherDataAggregator shall
// store 2 entries in WeatherHistory
```

As you can see, the WeatherDataAggregator's GetNotifiedAboutNewData method shall be triggered once the Measure method from WatherBallon or WeatherStation is used. This method simply adds a new WeatherData object to the WeatherHistory collection. This way, at the end of this code, the WeatherDataAggregator's WeatherHistory shall contain two entries.

Your job is to finalize this code:

1) In WeatherDataAggregator, implement the GetNotifiedAboutNewData. Its parameter shall be the sender of the event and a proper EventArgs type object. It should add the WeatherData item stored in the eventArgs to the _weatherHistory List.

2) In the WeatherStation and WeatherBaloon methods, implement the OnWeatherMeasured methods. Those methods should raise the WeatherMeasured event with proper arguments (sender and event arguments).