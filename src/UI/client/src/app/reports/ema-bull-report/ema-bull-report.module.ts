import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmaBullReportComponent } from './ema-bull-report.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [EmaBullReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HighchartsChartModule, 
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    EmaBullReportComponent
  ]
})
export class EmaReportModule { }
