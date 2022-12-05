import { TestBed } from '@angular/core/testing';

import { MainframeIdTokenService } from './mainframe-id-token.service';

describe('MainframeIdTokenService', () => {
  let service: MainframeIdTokenService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MainframeIdTokenService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
