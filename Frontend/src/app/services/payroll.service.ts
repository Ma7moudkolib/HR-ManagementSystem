import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PayrollService {
  private apiUrl = `${environment.apiBaseUrl}/payroll`;

  constructor(private http: HttpClient) {}

  getPayrollSummaries(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/summaries`);
  }

  getPayrollRecords(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/records`);
  }

  getPayrollRecordsByPeriod(periodId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/records/period/${periodId}`);
  }

  getPayrollByEmployeeId(employeeId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${employeeId}`);
  }

  generatePayroll(periodData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/generate`, periodData);
  }
}
