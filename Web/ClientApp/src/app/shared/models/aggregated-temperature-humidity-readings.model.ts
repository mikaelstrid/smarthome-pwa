export interface IAggregatedTemperatureHumidityReadings {
   sensorId: string;
   count: number;
   fromWest: Date;
   toWest: Date;
   averageTemperature: number;
   averageHumidity: number;
}
