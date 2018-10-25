import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../shared/services/signalr.service';

@Component({
   selector: 'app-temperature',
   templateUrl: './temperature.component.html',
   styleUrls: ['./temperature.component.scss'],
})
export class TemperatureComponent implements OnInit {
   constructor(private signalRService: SignalRService) {}

   ngOnInit() {
      this.signalRService.init();
   }
}
