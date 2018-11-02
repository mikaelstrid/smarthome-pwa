import { Component, OnInit, Input } from '@angular/core';
import { ClimateService } from '../../../shared/services/climate.service';

@Component({
   selector: 'app-sensor-temperature-graph',
   templateUrl: './sensor-temperature-graph.component.html',
   styleUrls: ['./sensor-temperature-graph.component.scss']
})
export class SensorTemperatureGraphComponent implements OnInit {
   @Input()
   sensorId: string;

   constructor(private climateService: ClimateService) {}

   ngOnInit() {
      const now = new Date();
      this.climateService
         .getTemperatureHumidity(this.sensorId, now, now)
         .subscribe(readings => console.table(readings), error => console.log(error));
   }
}
