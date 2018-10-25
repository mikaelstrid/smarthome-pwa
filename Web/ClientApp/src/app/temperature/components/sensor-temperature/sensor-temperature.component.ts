import { Component, OnInit, Input } from '@angular/core';
import { ClimateService } from '../../../shared/services/climate.service';
import { ITemperatureHumidityReading } from '../../../shared/models/temperature-humidity-reading.model';
import { SignalRService } from '../../../shared/services/signalr.service';

@Component({
   selector: 'app-sensor-temperature',
   templateUrl: './sensor-temperature.component.html',
   styleUrls: ['./sensor-temperature.component.scss']
})
export class SensorTemperatureComponent implements OnInit {
   @Input()
   sensorId: string;
   latestReading: ITemperatureHumidityReading;

   constructor(private climateService: ClimateService, private signalRService: SignalRService) {}

   ngOnInit() {
      this.climateService
         .getLatestTemperatureHumidity(this.sensorId)
         .subscribe(reading => (this.latestReading = reading), error => console.log(error));

      this.signalRService.temperatureHumidityReadings.subscribe(
         reading => (this.latestReading = reading)
      );
   }
}
