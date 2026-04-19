export interface CreateLeaveRequestDto {
  employeeId: string;
  leaveTypeId: string;
  startDate: string; // ISO dates
  endDate: string; // ISO dates
  reason: string;
}

export interface LeaveRequestDto {
  id: string;
  employeeId: string;
  leaveTypeId: string;
  startDate: string;
  endDate: string;
  reason: string;
  status: 'Pending' | 'Approved' | 'Rejected';
}

export interface LeaveBalanceDto {
  employeeId: string;
  remainingDays: number;
  paidDaysUsed: number;
  unpaidDaysUsed: number;
}
