import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HighchartsChartModule } from 'highcharts-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';
import { SmaBearReportComponent } from './sma-bear-report.component';


@NgModule({
  declarations: [SmaBearReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HighchartsChartModule, 
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    SmaBearReportComponent
  ]
})
export class SmaBearReportModule { }
