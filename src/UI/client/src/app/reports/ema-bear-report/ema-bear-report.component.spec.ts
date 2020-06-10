import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmaBearReportComponent } from './ema-bear-report.component';

describe('EmaBearReportComponent', () => {
  let component: EmaBearReportComponent;
  let fixture: ComponentFixture<EmaBearReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmaBearReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmaBearReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
