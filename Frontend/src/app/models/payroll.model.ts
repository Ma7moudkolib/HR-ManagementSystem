export interface PayrollSummaryDto {
  id: string;
  employeeId: string;
  year: number;
  month: number;
  totalBasicSalary: number;
  totalNetSalary: number;
}

export interface GeneratePayrollRequestDto {
  companyId: string;
  year: number;
  month: number;
}
