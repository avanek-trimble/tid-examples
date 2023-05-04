import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WeatherForecastRoutingModule } from './weather-forecast-routing.module';
import { WeatherForecastComponent } from './weather-forecast.component';


@NgModule({
  declarations: [
    WeatherForecastComponent
  ],
  imports: [
    CommonModule,
    WeatherForecastRoutingModule
  ]
})
export class WeatherForecastModule { }
