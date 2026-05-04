import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/auth/login/login.component';
import { CompanyListComponent } from './features/company/company-list/company-list.component';
import { EmployeeListComponent } from './features/employees/employee-list/employee-list.component';
import { AttendanceTrackerComponent } from './features/hr-dashboard/attendance-tracker/attendance-tracker.component';
import { LeaveManagerComponent } from './features/hr-dashboard/leave-manager/leave-manager.component';
import { PayrollSummaryComponent } from './features/hr-dashboard/payroll-summary/payroll-summary.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { AuthService } from './services/auth.service';
import { CompanyService } from './services/company.service';
import { EmployeeService } from './services/employee.service';
import { LeaveService } from './services/leave.service';
import { AttendanceService } from './services/attendance.service';
import { PayrollService } from './services/payroll.service';

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
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    AuthService,
    CompanyService,
    EmployeeService,
    LeaveService,
    AttendanceService,
    PayrollService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
