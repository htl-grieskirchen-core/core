import { TestBed } from '@angular/core/testing';

import { MainframeService } from './mainframe.service';

describe('MainframeService', () => {
  let service: MainframeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MainframeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
