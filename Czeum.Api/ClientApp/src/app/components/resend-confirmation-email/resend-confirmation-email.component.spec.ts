import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResendConfirmationEmailComponent } from './resend-confirmation-email.component';

describe('ResendConfirmationEmailComponent', () => {
  let component: ResendConfirmationEmailComponent;
  let fixture: ComponentFixture<ResendConfirmationEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResendConfirmationEmailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResendConfirmationEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
