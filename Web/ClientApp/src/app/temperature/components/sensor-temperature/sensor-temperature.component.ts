import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sensor-temperature',
  templateUrl: './sensor-temperature.component.html',
  styleUrls: ['./sensor-temperature.component.scss']
})
export class SensorTemperatureComponent implements OnInit {
   @Input() sensorId: string;

  constructor() { }

  ngOnInit() {
  }
}
