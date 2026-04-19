import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { EmployeeDto } from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class MockEmployeeService {
  private employees: EmployeeDto[] = [
    { id: '1', name: 'Eleanor Vance', age: 34, position: 'Senior Architect', email: 'eleanor.vance@nexus.com', salary: 125000, departmentId: 'd1' },
    { id: '2', name: 'Markus Ren', age: 28, position: 'Frontend Developer', email: 'markus.ren@nexus.com', salary: 95000, departmentId: 'd2' },
    { id: '3', name: 'Sarah OConnor', age: 41, position: 'HR Manager', email: 'sarah.o@nexus.com', salary: 110000, departmentId: 'd3' }
  ];

  getEmployees(): Observable<EmployeeDto[]> {
    return of(this.employees);
  }
}
