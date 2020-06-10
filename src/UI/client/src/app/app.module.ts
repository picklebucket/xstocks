import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmaReportModule } from './reports/ema-bull-report/ema-bull-report.module';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { HttpClientModule } from '@angular/common/http';
import { RocReportComponent } from './reports/roc-report/roc-report.component';
import { EmaBearReportComponent } from './reports/ema-bear-report/ema-bear-report.component';
import { EmaBearReportModule } from './reports/ema-bear-report/ema-bear-report.module';

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
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
