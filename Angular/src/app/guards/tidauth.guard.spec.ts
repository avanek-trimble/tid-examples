import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { tidAuthGuard } from './tidauth.guard';

describe('tidAuthGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => tidAuthGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
