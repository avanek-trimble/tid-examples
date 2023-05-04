import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';

import { WeatherForecastRoutingModule } from './weather-forecast-routing.module';
import { WeatherForecastComponent } from './weather-forecast.component';


@NgModule({
  declarations: [
    WeatherForecastComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatTableModule,
    WeatherForecastRoutingModule
  ]
})
export class WeatherForecastModule { }
