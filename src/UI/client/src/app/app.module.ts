import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmaReportModule } from './reports/ema-bull-report/ema-bull-report.module';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { HttpClientModule } from '@angular/common/http';
import { EmaBearReportModule } from './reports/ema-bear-report/ema-bear-report.module';
import { SmaBearReportModule } from './reports/sma-bear-report/sma-bear-report.module';
import { SmaBullReportModule } from './reports/sma-bull-report/sma-bull-report.module';
import { IchiReportModule } from './reports/ichi-report/ichi-report.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    EmaReportModule,
    EmaBearReportModule,
    CoreModule,
    SharedModule,
    SmaBearReportModule,
    SmaBullReportModule,
    IchiReportModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
