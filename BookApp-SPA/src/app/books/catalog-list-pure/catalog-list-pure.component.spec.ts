import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogListPureComponent } from './catalog-list-pure.component';

describe('CatalogListPureComponent', () => {
  let component: CatalogListPureComponent;
  let fixture: ComponentFixture<CatalogListPureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogListPureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogListPureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
