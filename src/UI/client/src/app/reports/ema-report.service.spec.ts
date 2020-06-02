import { TestBed } from '@angular/core/testing';

import { EmaReportService } from './ema-report.service';

describe('EmaReportService', () => {
  let service: EmaReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmaReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
