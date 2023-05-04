import { Component } from '@angular/core';
import { WeatherForecastService } from '../weather-forecast.service';
import { Observable } from 'rxjs';
import { WeatherForecast } from '../models/weather-forecast';

@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrls: ['./weather-forecast.component.scss']
})
export class WeatherForecastComponent {

  forecastTableColumns = ['date', 'temperatureC', 'temperatureF', 'summary'];
  forecast$: Observable<WeatherForecast[]> | undefined;

  public constructor(private weatherForecastService: WeatherForecastService) {}

  public retrieveForecast(): void {
    this.forecast$ = this.weatherForecastService.getWeatherForecast();
  }
}
