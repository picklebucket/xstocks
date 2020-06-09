import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmaReportComponent } from './reports/ema-report/ema-report.component';
import { RocReportComponent } from './reports/roc-report/roc-report.component';


const routes: Routes = [
  {path: 'ema-report', component: EmaReportComponent},
  {path: 'roc-report', component: RocReportComponent},
  {path: '**', redirectTo: '/ema-report'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
