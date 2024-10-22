import { TestBed } from '@angular/core/testing';

import { BreadService } from './bread.service';

describe('BreadService', () => {
  let service: BreadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BreadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
