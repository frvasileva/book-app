import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBookPreferencesComponent } from './user-book-preferences.component';

describe('UserBookPreferencesComponent', () => {
  let component: UserBookPreferencesComponent;
  let fixture: ComponentFixture<UserBookPreferencesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBookPreferencesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBookPreferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
