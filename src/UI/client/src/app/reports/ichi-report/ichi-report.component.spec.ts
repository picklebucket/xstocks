import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IchiReportComponent } from './ichi-report.component';

describe('IchiReportComponent', () => {
  let component: IchiReportComponent;
  let fixture: ComponentFixture<IchiReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IchiReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IchiReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
