import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmaReportComponent } from './reports/ema-report/ema-report.component';


const routes: Routes = [
  {path: 'ema-report', component: EmaReportComponent},
  {path: '**', redirectTo: '/ema-report'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
