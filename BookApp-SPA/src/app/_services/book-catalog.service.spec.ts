import { TestBed } from '@angular/core/testing';

import { BookCatalogService } from './book-catalog.service';

describe('BookCatalogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BookCatalogService = TestBed.get(BookCatalogService);
    expect(service).toBeTruthy();
  });
});
