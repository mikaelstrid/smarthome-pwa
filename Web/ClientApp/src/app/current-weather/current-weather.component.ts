import { Component, OnInit, Input } from '@angular/core';
import { ClimateService } from '../shared/services/climate.service';
import { ICurrentWeatherReport } from '../shared/models/current-weather-report.model';

@Component({
   selector: 'app-current-weather',
   templateUrl: './current-weather.component.html',
   styleUrls: ['./current-weather.component.scss'],
})
export class CurrentWeatherComponent implements OnInit {
   city = 'lindome';
   currentWeatherReport: ICurrentWeatherReport;

   constructor(private climateService: ClimateService) {}

   ngOnInit() {
      this.climateService
         .getCurrentWeather(this.city)
         .subscribe(report => (this.currentWeatherReport = report), error => console.log(error));
   }
}
