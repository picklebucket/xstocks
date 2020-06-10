import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmaBullReportComponent } from './ema-bull-report.component';

describe('EmaReportComponent', () => {
  let component: EmaBullReportComponent;
  let fixture: ComponentFixture<EmaBullReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmaBullReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmaBullReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
 