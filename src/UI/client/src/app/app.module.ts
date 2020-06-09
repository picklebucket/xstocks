import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmaReportModule } from './reports/ema-report/ema-report.module';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { HttpClientModule } from '@angular/common/http';
import { RocReportComponent } from './reports/roc-report/roc-report.component';

@NgModule({
  declarations: [
    AppComponent,
    RocReportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    EmaReportModule,
    CoreModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
