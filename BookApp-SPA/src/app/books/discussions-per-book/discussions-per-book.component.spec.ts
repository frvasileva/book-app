import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiscussionsPerBookComponent } from './discussions-per-book.component';

describe('DiscussionsPerBookComponent', () => {
  let component: DiscussionsPerBookComponent;
  let fixture: ComponentFixture<DiscussionsPerBookComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiscussionsPerBookComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiscussionsPerBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
