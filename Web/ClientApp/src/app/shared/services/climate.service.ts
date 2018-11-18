import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ITemperatureHumidityReading } from '../models/temperature-humidity-reading.model';
import { HttpClient } from '@angular/common/http';
import { IAggregatedTemperatureHumidityReadings } from '../models/aggregated-temperature-humidity-readings.model';
import { ICurrentWeatherReport } from '../models/current-weather-report.model';

@Injectable()
export class ClimateService {
   constructor(private httpClient: HttpClient) {}

   useMockData = false;

   getLatestTemperatureHumidity(sensorId: string): Observable<ITemperatureHumidityReading> {
      if (!this.useMockData) {
         return this.httpClient.get<ITemperatureHumidityReading>(
            `/api/climate/${sensorId}/latest-temperature-humidity`
         );
      }
      return of({
         sensorId: '60:01:94:43:2D:08',
         timestampWest: new Date(),
         temperature: 23.5,
         humidity: 45.6
      });
   }

   getTemperatureHumidity(
      sensorId: string,
      fromWest: Date,
      toWest: Date
   ): Observable<IAggregatedTemperatureHumidityReadings[]> {
      if (!this.useMockData) {
         return this.httpClient.get<IAggregatedTemperatureHumidityReadings[]>(
            // tslint:disable-next-line:max-line-length
            `/api/climate/${sensorId}/temperature-humidity?sensorId=${sensorId}&fromWest=${fromWest.toISOString()}&toWest=${toWest.toISOString()}`
         );
      }
      return of([
         {
            sensorId: '60:01:94:43:2D:08',
            count: 3,
            fromWest: '2018-11-02T08:00:00+01:00',
            toWest: '2018-11-02T09:00:00+01:00',
            averageTemperature: 22.7,
            averageHumidity: 43.4
         },
         {
            sensorId: '60:01:94:43:2D:08',
            count: 3,
            fromWest: '2018-11-02T09:00:00+01:00',
            toWest: '2018-11-02T10:00:00+01:00',
            averageTemperature: 22.5,
            averageHumidity: 43.7
         },
         {
            sensorId: '60:01:94:43:2D:08',
            count: 3,
            fromWest: '2018-11-02T10:00:00+01:00',
            toWest: '2018-11-02T11:00:00+01:00',
            averageTemperature: 22.4,
            averageHumidity: 43.5
         },
         {
            sensorId: '60:01:94:43:2D:08',
            count: 3,
            fromWest: '2018-11-02T11:00:00+01:00',
            toWest: '2018-11-02T12:00:00+01:00',
            averageTemperature: 22.7,
            averageHumidity: 43.4
         }
      ]);
   }

   getCurrentWeather(city: string): Observable<ICurrentWeatherReport> {
      if (!this.useMockData) {
         return this.httpClient.get<ICurrentWeatherReport>(
            `/api/climate/${city}/current-weather`
         );
      }
      return of({
         city: 'lindome',
         timestampWest: new Date(),
         weatherId: 803,
         weatherDescription: 'molningt',
         weatherIcon: '04n',
         temperature: 3
      });
   }
}
