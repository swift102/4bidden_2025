import { TestBed } from '@angular/core/testing';

import { Alert } from '../app/Models/alert.model';


describe('Alert', () => {
  let service: Alert;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Alert);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
