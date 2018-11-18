import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { SensorTemperatureComponent } from './temperature/components/sensor-temperature/sensor-temperature.component';
import { ClimateService } from './shared/services/climate.service';
import { SignalRService } from './shared/services/signalr.service';
import { SensorTemperatureGraphComponent } from './temperature/components/sensor-temperature-graph/sensor-temperature-graph.component';
import { CurrentWeatherComponent } from './current-weather/current-weather.component';

@NgModule({
   declarations: [AppComponent, TemperatureComponent, SensorTemperatureComponent, SensorTemperatureGraphComponent, CurrentWeatherComponent],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      HttpClientModule,
      RouterModule.forRoot([
         // { path: '', component: HomeComponent, pathMatch: 'full' }
         { path: 'temperatur', component: TemperatureComponent },
         { path: 'vader', component: CurrentWeatherComponent },
         { path: '**', redirectTo: 'temperatur' },
      ]),
   ],
   providers: [ClimateService, SignalRService],
   bootstrap: [AppComponent],
})
export class AppModule {}
