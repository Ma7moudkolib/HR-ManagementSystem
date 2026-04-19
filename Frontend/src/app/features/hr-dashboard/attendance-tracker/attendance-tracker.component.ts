import { Component, OnInit } from '@angular/core';
import { AttendanceDto } from '../../../models/attendance.model';
import { MockDataService } from '../../../services/mock-data.service';

@Component({
  selector: 'app-attendance-tracker',
  templateUrl: './attendance-tracker.component.html',
  styleUrls: ['./attendance-tracker.component.css']
})
export class AttendanceTrackerComponent implements OnInit {
  attendances: AttendanceDto[] = [];
  currentEmployeeId = '1'; // Mock logged in employee

  constructor(private dataService: MockDataService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.dataService.getAttendances().subscribe(data => {
      this.attendances = data;
    });
  }

  punchIn() {
    this.dataService.punchIn(this.currentEmployeeId);
    this.loadData();
  }

  punchOut(recordId: string) {
    this.dataService.punchOut(recordId);
    this.loadData();
  }

  hasActiveSession() {
    return this.attendances.some(a => a.employeeId === this.currentEmployeeId && !a.checkOut);
  }

  getActiveRecord() {
    return this.attendances.find(a => a.employeeId === this.currentEmployeeId && !a.checkOut);
  }
}
