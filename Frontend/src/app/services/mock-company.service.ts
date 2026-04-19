import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { CompanyDto } from '../models/company.model';

@Injectable({
  providedIn: 'root'
})
export class MockCompanyService {
  private companies: CompanyDto[] = [
    { id: '1', name: 'Nexus Design Studio', address: '123 Innovation Way', country: 'United States' },
    { id: '2', name: 'Aether Labs Ltd.', address: '456 Tech Park', country: 'United Kingdom' },
    { id: '3', name: 'Horizon Holdings', address: '789 Business Rd', country: 'Canada' },
    { id: '4', name: 'Velocity Ventures', address: '101 Startup Blvd', country: 'Germany' }
  ];

  getCompanies(): Observable<CompanyDto[]> {
    return of(this.companies);
  }
}
