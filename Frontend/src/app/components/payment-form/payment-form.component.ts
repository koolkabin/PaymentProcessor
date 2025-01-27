import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-payment-form',
  templateUrl: './payment-form.component.html',
  styleUrls: ['./payment-form.component.css']
})
export class PaymentFormComponent {
  paymentData = {
    amount: 0,
    currency: 'USD',
    name: '',
    email: '',
    cardNumber: '',
    expiryDate: '',
    cvv: ''
  };

  constructor(private apiService: ApiService) {}

  submitPayment() {
    this.apiService.processPayment(this.paymentData).subscribe({
      next: (response) => alert('Payment initiated!'),
      error: (err) => alert('Error processing payment: ' + err.message)
    });
  }
}
