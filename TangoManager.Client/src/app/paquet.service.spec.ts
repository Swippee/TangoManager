import { TestBed } from '@angular/core/testing';

import { PaquetService } from './paquet.service';

describe('PaquetService', () => {
  let service: PaquetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaquetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
