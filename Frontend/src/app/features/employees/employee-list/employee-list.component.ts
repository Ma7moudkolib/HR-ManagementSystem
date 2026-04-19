import { Component, OnInit } from '@angular/core';
import { EmployeeDto } from '../../../models/employee.model';
import { MockEmployeeService } from '../../../services/mock-employee.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeDto[] = [];

  constructor(private employeeService: MockEmployeeService) {}

  ngOnInit(): void {
    this.employeeService.getEmployees().subscribe(data => {
      this.employees = data;
    });
  }

  deleteEmployee(id: string) {
    if(confirm('Are you sure you want to delete this employee?')) {
      this.employees = this.employees.filter(e => e.id !== id);
    }
  }
}
