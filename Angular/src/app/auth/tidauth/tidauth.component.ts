import { Component, OnInit } from '@angular/core';
import { LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, take } from 'rxjs';

@Component({
  selector: 'app-tidauth',
  templateUrl: './tidauth.component.html',
  styleUrls: ['./tidauth.component.scss']
})
export class TIDAuthComponent implements OnInit {

  authCheck$: Observable<LoginResponse> | undefined;

  public constructor(private oidcSvc: OidcSecurityService) {}
  
  ngOnInit(): void {
    // This will ensure the code -> token exchange occurs
    this.authCheck$ = this.oidcSvc.checkAuth();
  }

  public signin(): void {
    // This performs the call to TID to do the grant type
    // flow up until the code -> token exchange
    this.oidcSvc.authorize();
  }

  public signout(): void {
    // This library requires you to subscribe to the service method
    // for it to actually execute the logoff logic. The pipe/take
    // is an rxjs workaround for the subscribe call to not require
    // a call to unsubscribe.
    this.oidcSvc.logoff().pipe(take(1)).subscribe({ next: () => {}});
  }
}
