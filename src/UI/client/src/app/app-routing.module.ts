import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmaBullReportComponent } from './reports/ema-bull-report/ema-bull-report.component';
import { EmaBearReportComponent } from './reports/ema-bear-report/ema-bear-report.component';


const routes: Routes = [
  {path: 'ema-bull-report', component: EmaBullReportComponent},
  {path: 'ema-bear-report', component: EmaBearReportComponent},
  {path: '**', redirectTo: '/ema-bull-report'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
