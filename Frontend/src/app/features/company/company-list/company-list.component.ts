import { Component, OnInit } from '@angular/core';
import { CompanyDto } from '../../../models/company.model';
import { MockCompanyService } from '../../../services/mock-company.service';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  companies: CompanyDto[] = [];

  constructor(private companyService: MockCompanyService) {}

  ngOnInit(): void {
    this.companyService.getCompanies().subscribe(data => {
      this.companies = data;
    });
  }

  deleteCompany(id: string) {
    if(confirm('Are you sure you want to delete this company?')) {
      this.companies = this.companies.filter(c => c.id !== id);
    }
  }
}
