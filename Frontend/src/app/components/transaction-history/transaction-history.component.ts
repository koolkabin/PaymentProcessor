import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrls: ['./transaction-history.component.css']
})
export class TransactionHistoryComponent implements OnInit {
  transactions: any[] = [];

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.fetchTransactions();
    setInterval(() => this.fetchTransactions(), 5000); // Poll every 5 seconds
  }

  fetchTransactions() {
    this.apiService.getTransactions().subscribe({
      next: (response) => (this.transactions = response),
      error: (err) => console.error('Error fetching transactions: ', err)
    });
  }
}
