import { TestBed } from '@angular/core/testing';

import { MainframeNavService } from './mainframe-nav.service';

describe('MainframeNavService', () => {
  let service: MainframeNavService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MainframeNavService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
