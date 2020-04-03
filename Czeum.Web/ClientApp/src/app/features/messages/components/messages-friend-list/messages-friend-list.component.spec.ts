import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagesFriendListComponent } from './messages-friend-list.component';

describe('MessagesFriendListComponent', () => {
  let component: MessagesFriendListComponent;
  let fixture: ComponentFixture<MessagesFriendListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessagesFriendListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagesFriendListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
