import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HighchartsChartModule } from 'highcharts-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';
import { SmaBullReportComponent } from './sma-bull-report.component';



@NgModule({
  declarations: [SmaBullReportComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HighchartsChartModule, 
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    SmaBullReportComponent
  ]
})
export class SmaBullReportModule { }
