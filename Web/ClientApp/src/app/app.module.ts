import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { SensorTemperatureComponent } from './temperature/components/sensor-temperature/sensor-temperature.component';
import { ClimateService } from './shared/services/climate.service';
import { SignalRService } from './shared/services/signalr.service';

@NgModule({
   declarations: [AppComponent, TemperatureComponent, SensorTemperatureComponent],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      HttpClientModule,
      RouterModule.forRoot([
         // { path: '', component: HomeComponent, pathMatch: 'full' }
         { path: 'temperatur', component: TemperatureComponent },
         { path: '**', redirectTo: 'temperatur' },
      ]),
   ],
   providers: [ClimateService, SignalRService],
   bootstrap: [AppComponent],
})
export class AppModule {}
