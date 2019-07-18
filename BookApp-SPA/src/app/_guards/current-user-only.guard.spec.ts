import { TestBed, async, inject } from '@angular/core/testing';

import { CurrentUserOnlyGuard } from './current-user-only.guard';

describe('CurrentUserOnlyGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CurrentUserOnlyGuard]
    });
  });

  it('should ...', inject([CurrentUserOnlyGuard], (guard: CurrentUserOnlyGuard) => {
    expect(guard).toBeTruthy();
  }));
});
