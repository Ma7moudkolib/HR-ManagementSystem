import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/auth/login/login.component';
import { CompanyListComponent } from './features/company/company-list/company-list.component';
import { EmployeeListComponent } from './features/employees/employee-list/employee-list.component';
import { AttendanceTrackerComponent } from './features/hr-dashboard/attendance-tracker/attendance-tracker.component';
import { LeaveManagerComponent } from './features/hr-dashboard/leave-manager/leave-manager.component';
import { PayrollSummaryComponent } from './features/hr-dashboard/payroll-summary/payroll-summary.component';
import { RegisterComponent } from './features/auth/register/register.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    CompanyListComponent,
    EmployeeListComponent,
    AttendanceTrackerComponent,
    LeaveManagerComponent,
    PayrollSummaryComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
