import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBookCoverComponent } from './add-book-cover.component';

describe('AddBookCoverComponent', () => {
  let component: AddBookCoverComponent;
  let fixture: ComponentFixture<AddBookCoverComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddBookCoverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddBookCoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
