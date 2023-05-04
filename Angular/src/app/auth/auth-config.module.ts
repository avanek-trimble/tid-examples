import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

import { TIDAuthComponent } from './tidauth/tidauth.component';

@NgModule({
    imports: [AuthModule.forRoot({
        config: {
              authority: 'https://stage.id.trimblecloud.com',
              redirectUrl: window.location.origin,
              postLogoutRedirectUri: window.location.origin,
              clientId: '4ce6f16c-f0c2-4c86-8388-db37511df6c1',
              scope: 'openid AV-TID-Example offline_access',
              responseType: 'code',
              silentRenew: true,
              useRefreshToken: true,
              renewTimeBeforeTokenExpiresInSeconds: 30,
          }
      }),
      CommonModule,
      MatButtonModule
    ],
    exports: [ TIDAuthComponent, AuthModule],
    declarations: [
      TIDAuthComponent
    ],
})
export class AuthConfigModule {}
