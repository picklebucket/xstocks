import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmaReportComponent } from './ema-report.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [EmaReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HighchartsChartModule, 
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    EmaReportComponent
  ]
})
export class EmaReportModule { }
