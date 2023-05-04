import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TIDAuthComponent } from './tidauth.component';

describe('TIDAuthComponent', () => {
  let component: TIDAuthComponent;
  let fixture: ComponentFixture<TIDAuthComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TIDAuthComponent]
    });
    fixture = TestBed.createComponent(TIDAuthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
