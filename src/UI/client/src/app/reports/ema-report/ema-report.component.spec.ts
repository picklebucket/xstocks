import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmaReportComponent } from './ema-report.component';

describe('EmaReportComponent', () => {
  let component: EmaReportComponent;
  let fixture: ComponentFixture<EmaReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmaReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmaReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
 