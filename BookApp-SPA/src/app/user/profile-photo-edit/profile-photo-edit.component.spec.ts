import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilePhotoEditComponent } from './profile-photo-edit.component';

describe('ProfilePhotoEditComponent', () => {
  let component: ProfilePhotoEditComponent;
  let fixture: ComponentFixture<ProfilePhotoEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfilePhotoEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfilePhotoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
