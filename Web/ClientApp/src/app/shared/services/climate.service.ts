import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ITemperatureHumidityReading } from '../models/temperature-humidity-reading.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ClimateService {
   constructor(private httpClient: HttpClient) {}

   getLatestTemperatureHumidity(sensorId: string): Observable<ITemperatureHumidityReading> {
      // return of({
      //    sensorId: '60:01:94:43:2D:08',
      //    timestampWest: new Date(),
      //    temperature: 23.5,
      //    humidity: 45.6
      // });
      return this.httpClient.get<ITemperatureHumidityReading>(
         `/api/climate/${sensorId}/latest-temperature-humidity`
      );
   }
}
