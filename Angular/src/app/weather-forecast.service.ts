import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map, switchMap } from 'rxjs';
import { WeatherForecast } from './models/weather-forecast';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable({
  providedIn: 'root'
})
export class WeatherForecastService {

  constructor(private httpClient: HttpClient, private oidcSvc: OidcSecurityService) { }

  private getAuthorizationHeader(): Observable<string> {
    return this.oidcSvc.getAccessToken().pipe(
      map(accessToken => (`Bearer ${accessToken}`))
    );
  }

  getWeatherForecast(): Observable<WeatherForecast[]> {
    return this.getAuthorizationHeader().pipe(
      switchMap(auth => {
        const httpHeaders = new HttpHeaders().set('Authorization', auth);
        return this.httpClient.get<WeatherForecast[]>('https://localhost:7227/WeatherForecast', { headers: httpHeaders });
      })
    );
  }
}
