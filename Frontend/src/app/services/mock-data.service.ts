import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { LeaveRequestDto, LeaveBalanceDto } from '../models/leave.model';
import { PayrollSummaryDto } from '../models/payroll.model';
import { AttendanceDto } from '../models/attendance.model';

@Injectable({
  providedIn: 'root'
})
export class MockDataService {
  private leaveRequests: LeaveRequestDto[] = [
    { id: 'lr1', employeeId: '1', leaveTypeId: 'Annual', startDate: '2026-05-01', endDate: '2026-05-05', reason: 'Vacation', status: 'Pending' },
    { id: 'lr2', employeeId: '2', leaveTypeId: 'Sick', startDate: '2026-04-10', endDate: '2026-04-12', reason: 'Flu', status: 'Approved' }
  ];

  private leaveBalances: LeaveBalanceDto[] = [
    { employeeId: '1', remainingDays: 14, paidDaysUsed: 5, unpaidDaysUsed: 0 },
    { employeeId: '2', remainingDays: 10, paidDaysUsed: 6, unpaidDaysUsed: 2 }
  ];

  private payrollSummaries: PayrollSummaryDto[] = [
    { id: 'p1', employeeId: '1', year: 2026, month: 3, totalBasicSalary: 10416, totalNetSalary: 8300 },
    { id: 'p2', employeeId: '2', year: 2026, month: 3, totalBasicSalary: 7916, totalNetSalary: 6100 }
  ];

  private attendances: AttendanceDto[] = [
    { id: 'a1', employeeId: '1', checkIn: new Date().toISOString() },
    { id: 'a2', employeeId: '2', checkIn: new Date(Date.now() - 3600*1000*8).toISOString(), checkOut: new Date().toISOString(), workedHours: 8 }
  ];

  getLeaveRequests(): Observable<LeaveRequestDto[]> {
    return of(this.leaveRequests);
  }
  
  getLeaveBalances(): Observable<LeaveBalanceDto[]> {
    return of(this.leaveBalances);
  }

  getPayrollSummaries(): Observable<PayrollSummaryDto[]> {
    return of(this.payrollSummaries);
  }

  getAttendances(): Observable<AttendanceDto[]> {
    return of([...this.attendances]);
  }

  updateLeaveStatus(id: string, newStatus: 'Pending' | 'Approved' | 'Rejected') {
    const req = this.leaveRequests.find(r => r.id === id);
    if(req) req.status = newStatus;
  }

  punchIn(employeeId: string) {
    const newRecord: AttendanceDto = {
      id: Math.random().toString(36).substr(2, 9),
      employeeId,
      checkIn: new Date().toISOString()
    };
    this.attendances.push(newRecord);
  }

  punchOut(recordId: string) {
    const record = this.attendances.find(a => a.id === recordId && !a.checkOut);
    if(record) {
      record.checkOut = new Date().toISOString();
      // Calculate diff in hours
      const diff = new Date(record.checkOut).getTime() - new Date(record.checkIn).getTime();
      record.workedHours = diff / (1000 * 60 * 60);
    }
  }
}
