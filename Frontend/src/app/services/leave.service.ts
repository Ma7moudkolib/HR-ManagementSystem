import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
  private apiUrl = `${environment.apiBaseUrl}/leave`;

  constructor(private http: HttpClient) {}

  getLeaveRequests(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/requests`);
  }

  createLeaveRequest(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/requests`, request);
  }

  updateLeaveRequest(id: string, request: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/requests/${id}`, request);
  }

  deleteLeaveRequest(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/requests/${id}`);
  }

  getLeaveBalance(employeeId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/balance/${employeeId}`);
  }

  getLeaveBalances(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/balances`);
  }
}
