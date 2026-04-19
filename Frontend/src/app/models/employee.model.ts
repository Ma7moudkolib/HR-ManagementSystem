export interface EmployeeDto {
  id: string;
  name: string;
  age: number;
  position: string;
  email: string;
  salary: number;
  departmentId: string;
}

export interface EmployeeForCreationDto {
  name: string;
  age: number;
  position: string;
  email: string;
  salary: number;
  departmentId: string;
}
