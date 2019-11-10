import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceivedRequestListComponent } from './received-request-list.component';

describe('ReceivedRequestListComponent', () => {
  let component: ReceivedRequestListComponent;
  let fixture: ComponentFixture<ReceivedRequestListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReceivedRequestListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceivedRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
