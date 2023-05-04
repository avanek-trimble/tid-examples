import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';


@NgModule({
    imports: [AuthModule.forRoot({
        config: {
              authority: 'https://stage.id.trimblecloud.com',
              redirectUrl: window.location.origin,
              postLogoutRedirectUri: window.location.origin,
              clientId: '4ce6f16c-f0c2-4c86-8388-db37511df6c1',
              scope: 'openid AV-TID-Example',
              responseType: 'code',
              silentRenew: true,
              useRefreshToken: true,
              renewTimeBeforeTokenExpiresInSeconds: 30,
          }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
