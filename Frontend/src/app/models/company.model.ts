export interface CompanyDto {
  id: string;
  name: string;
  address: string;
  country: string;
}

export interface CompanyForCreationDto {
  name: string;
  address: string;
  country: string;
}

export interface CompanyForUpdateDto {
  name: string;
  address: string;
  country: string;
}
