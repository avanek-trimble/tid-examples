import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map, take } from 'rxjs';

export const tidAuthGuard: CanActivateFn = (route, state) => {
  const oidcSvc = inject(OidcSecurityService);
  const router = inject(Router);
  
  return oidcSvc.isAuthenticated$.pipe(
    take(1),
    map(i => {
      if (!i.isAuthenticated) {
        return router.parseUrl('/unauthorized');  
      }

      return true;
    })
  );
};
