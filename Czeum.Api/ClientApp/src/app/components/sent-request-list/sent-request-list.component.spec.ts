import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SentRequestListComponent } from './sent-request-list.component';

describe('SentRequestListComponent', () => {
  let component: SentRequestListComponent;
  let fixture: ComponentFixture<SentRequestListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SentRequestListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SentRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
