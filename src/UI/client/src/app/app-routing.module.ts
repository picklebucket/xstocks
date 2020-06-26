import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmaBullReportComponent } from './reports/ema-bull-report/ema-bull-report.component';
import { EmaBearReportComponent } from './reports/ema-bear-report/ema-bear-report.component';
import { SmaBullReportComponent } from './reports/sma-bull-report/sma-bull-report.component';
import { SmaBearReportComponent } from './reports/sma-bear-report/sma-bear-report.component';
import { IchiReportComponent } from './reports/ichi-report/ichi-report.component';


const routes: Routes = [
  {path: 'ema-bull-report', component: EmaBullReportComponent},
  {path: 'ema-bear-report', component: EmaBearReportComponent},
  {path: 'sma-bull-report', component: SmaBullReportComponent},
  {path: 'sma-bear-report', component: SmaBearReportComponent},
  {path: 'ichi-report', component: IchiReportComponent},
  {path: '**', redirectTo: '/ema-bull-report'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
