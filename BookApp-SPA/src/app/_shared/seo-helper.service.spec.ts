import { TestBed } from '@angular/core/testing';

import { SeoHelperService } from './seo-helper.service';

describe('SeoHelperService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SeoHelperService = TestBed.get(SeoHelperService);
    expect(service).toBeTruthy();
  });
});
