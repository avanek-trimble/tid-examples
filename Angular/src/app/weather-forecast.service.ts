import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WeatherForecast } from './models/weather-forecast';

@Injectable({
  providedIn: 'root'
})
export class WeatherForecastService {

  constructor(private httpClient: HttpClient ) { }

  getWeatherForecast(): Observable<WeatherForecast[]> {
    return this.httpClient.get<WeatherForecast[]>('https://localhost:7227/WeatherForecast');
  }
}
