import { Component, OnInit, Input } from '@angular/core';
import { Chart } from 'chart.js';
import * as dateFns from 'date-fns';
import { ClimateService } from '../../../shared/services/climate.service';
import { IAggregatedTemperatureHumidityReadings } from '../../../shared/models/aggregated-temperature-humidity-readings.model';

@Component({
   selector: 'app-sensor-temperature-graph',
   templateUrl: './sensor-temperature-graph.component.html',
   styleUrls: ['./sensor-temperature-graph.component.scss'],
})
export class SensorTemperatureGraphComponent implements OnInit {
   @Input()
   sensorId: string;
   chart: Chart = undefined;

   constructor(private climateService: ClimateService) {}

   ngOnInit() {
      const now = new Date();

      this.climateService.getTemperatureHumidity(this.sensorId, dateFns.subDays(now, 1), now).subscribe(
         readings => {
            this.updateChart(readings);
         },
         error => console.log(error)
      );
   }

   private updateChart(data: IAggregatedTemperatureHumidityReadings[]) {
      this.chart = new Chart('canvas', {
         type: 'line',
         data: {
            labels: data.map(reading =>
               new Date(reading.fromWest).toLocaleTimeString('sv', { hour: 'numeric', minute: 'numeric' })),
            datasets: [
               {
                  data: data.map<number>(reading => reading.averageTemperature),
                  borderColor: '#3cba9f',
                  fill: false,
                  pointRadius: 0
               },
            ],
         },
         options: {
            legend: {
               display: false,
            },
            scales: {
               xAxes: [
                  {
                     display: true,
                     gridLines: {
                        display: false
                     }
                  },
               ],
               yAxes: [
                  {
                     display: true,
                     gridLines: {
                        display: false
                     }
                  },
               ],
            },
         },
      });
   }
}
