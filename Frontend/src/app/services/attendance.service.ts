import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = `${environment.apiBaseUrl}/attendance`;

  constructor(private http: HttpClient) {}

  getAttendances(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getAttendanceByEmployeeId(employeeId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/employee/${employeeId}`);
  }

  createAttendance(attendance: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, attendance);
  }

  updateAttendance(id: string, attendance: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, attendance);
  }

  checkIn(employeeId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/checkin`, { employeeId });
  }

  checkOut(employeeId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/checkout`, { employeeId });
  }
}
