import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookCardItemComponent } from './book-card-item.component';

describe('BookCardItemComponent', () => {
  let component: BookCardItemComponent;
  let fixture: ComponentFixture<BookCardItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookCardItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookCardItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
