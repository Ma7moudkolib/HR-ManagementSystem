import { Component, OnInit } from '@angular/core';
import { LeaveRequestDto, LeaveBalanceDto } from '../../../models/leave.model';
import { MockDataService } from '../../../services/mock-data.service';

@Component({
  selector: 'app-leave-manager',
  templateUrl: './leave-manager.component.html',
  styleUrls: ['./leave-manager.component.css']
})
export class LeaveManagerComponent implements OnInit {
  leaveRequests: LeaveRequestDto[] = [];
  leaveBalances: LeaveBalanceDto[] = [];
  viewMode: 'admin' | 'employee' = 'admin'; // toggle for UI test

  constructor(private dataService: MockDataService) {}

  ngOnInit(): void {
    this.dataService.getLeaveRequests().subscribe(data => this.leaveRequests = data);
    this.dataService.getLeaveBalances().subscribe(data => this.leaveBalances = data);
  }

  get pendingRequests() {
    return this.leaveRequests.filter(r => r.status === 'Pending');
  }

  approveRequest(id: string) {
    this.dataService.updateLeaveStatus(id, 'Approved');
  }

  rejectRequest(id: string) {
    this.dataService.updateLeaveStatus(id, 'Rejected');
  }
}
