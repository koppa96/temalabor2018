import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailedFriendListComponent } from './detailed-friend-list.component';

describe('DetailedFriendListComponent', () => {
  let component: DetailedFriendListComponent;
  let fixture: ComponentFixture<DetailedFriendListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailedFriendListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailedFriendListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
