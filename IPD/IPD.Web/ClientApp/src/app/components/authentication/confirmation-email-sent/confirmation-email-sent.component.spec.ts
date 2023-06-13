import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationEmailSentComponent } from './confirmation-email-sent.component';

describe('ConfirmationEmailSentComponent', () => {
  let component: ConfirmationEmailSentComponent;
  let fixture: ComponentFixture<ConfirmationEmailSentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfirmationEmailSentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmationEmailSentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
