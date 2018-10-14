import { Component, OnInit, Input } from '@angular/core';
import { ClimateService } from '../../../shared/services/climate.service';

@Component({
   selector: 'app-sensor-temperature',
   templateUrl: './sensor-temperature.component.html',
   styleUrls: ['./sensor-temperature.component.scss'],
})
export class SensorTemperatureComponent implements OnInit {
   @Input()
   sensorId: string;
   latestTemperature: number;

   constructor(private climateService: ClimateService) {}

   ngOnInit() {
      this.climateService
         .getLatestTemperatureHumidity(this.sensorId)
         .subscribe(
            reading => (this.latestTemperature = reading.temperature),
            error => console.log(error)
         );
   }
}
