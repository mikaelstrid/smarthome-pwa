import { Component, OnInit, Input } from '@angular/core';
import { ClimateService } from '../../../shared/services/climate.service';
import { ITemperatureHumidityReading } from '../../../shared/models/temperature-humidity-reading.model';

@Component({
   selector: 'app-sensor-temperature',
   templateUrl: './sensor-temperature.component.html',
   styleUrls: ['./sensor-temperature.component.scss'],
})
export class SensorTemperatureComponent implements OnInit {
   @Input()
   sensorId: string;
   latestReading: ITemperatureHumidityReading;

   constructor(private climateService: ClimateService) {}

   ngOnInit() {
      this.climateService
         .getLatestTemperatureHumidity(this.sensorId)
         .subscribe(
            reading => this.latestReading = reading,
            error => console.log(error)
         );
   }
}
