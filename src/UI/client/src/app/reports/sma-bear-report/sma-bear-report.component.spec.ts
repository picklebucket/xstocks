import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SmaBearReportComponent } from './sma-bear-report.component';

describe('SmaBearReportComponent', () => {
  let component: SmaBearReportComponent;
  let fixture: ComponentFixture<SmaBearReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SmaBearReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SmaBearReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
