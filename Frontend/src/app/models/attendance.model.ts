export interface AttendanceDto {
  id: string;
  employeeId: string;
  checkIn: string; // ISO dates
  checkOut?: string;
  workedHours?: number;
}
