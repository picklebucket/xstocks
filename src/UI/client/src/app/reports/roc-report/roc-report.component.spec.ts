import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RocReportComponent } from './roc-report.component';

describe('RocReportComponent', () => {
  let component: RocReportComponent;
  let fixture: ComponentFixture<RocReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RocReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RocReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
