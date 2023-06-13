import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoggedOutLayoutComponent } from './logged-out-layout.component';

describe('LoggedOutLayoutComponent', () => {
  let component: LoggedOutLayoutComponent;
  let fixture: ComponentFixture<LoggedOutLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoggedOutLayoutComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoggedOutLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
