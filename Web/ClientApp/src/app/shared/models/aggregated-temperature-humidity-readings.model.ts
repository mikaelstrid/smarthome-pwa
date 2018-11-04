export interface IAggregatedTemperatureHumidityReadings {
   sensorId: string;
   count: number;
   fromWest: string;
   toWest: string;
   averageTemperature: number;
   averageHumidity: number;
}
