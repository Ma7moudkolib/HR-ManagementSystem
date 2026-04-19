import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { CompanyListComponent } from './features/company/company-list/company-list.component';
import { EmployeeListComponent } from './features/employees/employee-list/employee-list.component';
import { AttendanceTrackerComponent } from './features/hr-dashboard/attendance-tracker/attendance-tracker.component';
import { LeaveManagerComponent } from './features/hr-dashboard/leave-manager/leave-manager.component';
import { PayrollSummaryComponent } from './features/hr-dashboard/payroll-summary/payroll-summary.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'companies', component: CompanyListComponent, canActivate: [AuthGuard] },
  { path: 'employees', component: EmployeeListComponent, canActivate: [AuthGuard] },
  { path: 'attendance', component: AttendanceTrackerComponent, canActivate: [AuthGuard] },
  { path: 'leave', component: LeaveManagerComponent, canActivate: [AuthGuard] },
  { path: 'payroll', component: PayrollSummaryComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/companies', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
