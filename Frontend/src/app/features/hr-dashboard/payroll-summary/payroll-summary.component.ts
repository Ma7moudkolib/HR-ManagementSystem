import { Component, OnInit } from '@angular/core';
import { PayrollSummaryDto } from '../../../models/payroll.model';
import { MockDataService } from '../../../services/mock-data.service';

@Component({
  selector: 'app-payroll-summary',
  templateUrl: './payroll-summary.component.html',
  styleUrls: ['./payroll-summary.component.css']
})
export class PayrollSummaryComponent implements OnInit {
  summaries: PayrollSummaryDto[] = [];
  showModal = false;

  constructor(private dataService: MockDataService) {}

  ngOnInit(): void {
    this.dataService.getPayrollSummaries().subscribe(data => this.summaries = data);
  }

  openGenerateModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  generatePayroll() {
    // In actual impl, calls specific GeneratePayrollRequestDto based service
    alert('Payroll Generated successfully for target period.');
    this.closeModal();
  }
}
