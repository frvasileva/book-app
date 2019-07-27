import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicCatalogListComponent } from './public-catalog-list.component';

describe('PublicCatalogListComponent', () => {
  let component: PublicCatalogListComponent;
  let fixture: ComponentFixture<PublicCatalogListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PublicCatalogListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicCatalogListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
