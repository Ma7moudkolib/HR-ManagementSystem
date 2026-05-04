import { Component, OnInit } from '@angular/core';
import { CompanyDto } from '../../../models/company.model';
import { CompanyService } from '../../../services/company.service';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {
  companies: CompanyDto[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(private companyService: CompanyService) {}

  ngOnInit(): void {
    this.loadCompanies();
  }

  loadCompanies(): void {
    this.isLoading = true;
    this.companyService.getCompanies().subscribe({
      next: (data) => {
        this.companies = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load companies: ' + (err.error?.message || err.message);
        this.isLoading = false;
      }
    });
  }

  deleteCompany(id: string) {
    if(confirm('Are you sure you want to delete this company?')) {
      this.companyService.deleteCompany(id).subscribe({
        next: () => {
          this.companies = this.companies.filter(c => c.id !== id);
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete company: ' + (err.error?.message || err.message);
        }
      });
    }
  }
}

