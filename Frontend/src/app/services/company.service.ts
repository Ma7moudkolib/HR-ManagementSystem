import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CompanyDto, CompanyForCreationDto, CompanyForUpdateDto } from '../models/company.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = `${environment.apiBaseUrl}/companies`;

  constructor(private http: HttpClient) {}

  getCompanies(): Observable<CompanyDto[]> {
    return this.http.get<CompanyDto[]>(this.apiUrl);
  }

  getCompanyById(id: string): Observable<CompanyDto> {
    return this.http.get<CompanyDto>(`${this.apiUrl}/${id}`);
  }

  createCompany(company: CompanyForCreationDto): Observable<CompanyDto> {
    return this.http.post<CompanyDto>(this.apiUrl, company);
  }

  updateCompany(id: string, company: CompanyForUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, company);
  }

  deleteCompany(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
