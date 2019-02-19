import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookSaverComponent } from './book-saver.component';

describe('BookSaverComponent', () => {
  let component: BookSaverComponent;
  let fixture: ComponentFixture<BookSaverComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookSaverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookSaverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
