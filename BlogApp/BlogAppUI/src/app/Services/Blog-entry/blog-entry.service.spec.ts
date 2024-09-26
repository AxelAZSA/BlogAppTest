import { TestBed } from '@angular/core/testing';

import { BlogEntryService } from './blog-entry.service';

describe('BlogEntryService', () => {
  let service: BlogEntryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BlogEntryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
