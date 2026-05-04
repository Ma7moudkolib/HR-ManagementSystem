import { Component, OnInit } from '@angular/core';
import { EmployeeDto } from '../../../models/employee.model';
import { EmployeeService } from '../../../services/employee.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeDto[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load employees: ' + (err.error?.message || err.message);
        this.isLoading = false;
      }
    });
  }

  deleteEmployee(id: string) {
    if(confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.employees = this.employees.filter(e => e.id !== id);
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete employee: ' + (err.error?.message || err.message);
        }
      });
    }
  }
}

