import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmaBearReportComponent } from './ema-bear-report.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [EmaBearReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HighchartsChartModule, 
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    EmaBearReportComponent
  ]
})
export class EmaBearReportModule { }
