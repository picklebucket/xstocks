import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SmaBullReportComponent } from './sma-bull-report.component';

describe('SmaBullReportComponent', () => {
  let component: SmaBullReportComponent;
  let fixture: ComponentFixture<SmaBullReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SmaBullReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SmaBullReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
