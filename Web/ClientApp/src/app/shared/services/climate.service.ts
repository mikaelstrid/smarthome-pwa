import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ITemperatureHumidityReading } from '../models/temperature-humidity-reading.model';

@Injectable()
export class ClimateService {
   constructor() {}

   getLatestTemperatureHumidity(sensorId: string): Observable<ITemperatureHumidityReading> {
      return of({
         sensorId: '',
         timestampWest: new Date(),
         temperature: 25.4,
         humidity: 46.7,
      });
   }
}
