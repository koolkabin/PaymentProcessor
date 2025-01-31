import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WebSocketSubject, webSocket } from 'rxjs/webSocket';

@Component({
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  formSubmitting = signal<boolean>(false);
  waitingResponse = signal<boolean>(false);
  title = 'smartpay';
  /**
    * Represents the form group for the payment form in the checkout component.
    * This form group will contain various form controls related to payment details.
    */
  paymentForm = new FormGroup({
    customerInfo: new FormGroup({
      name: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      phone: new FormControl('', Validators.required)
    }),
    cardInfo: new FormGroup({
      cardNumber: new FormControl(''),
      expiryDate: new FormControl(''),
      cvv: new FormControl('')
    }),
    bankInfo: new FormGroup({
      accountNumber: new FormControl(''),
      routingNumber: new FormControl('')
    }),
    paymentInfo: new FormGroup({
      amount: new FormControl('', Validators.required),
      currency: new FormControl('', Validators.required)
    }),
    paymentMethod: new FormControl('bank', Validators.required)
  });

  constructor(private http: HttpClient){}
  //, private websocketService: webSocketService) { }
  private socket$!: WebSocketSubject<any>;

  ngOnInit() {

    this.initializeSocketConnection();

    // Listen for payment method changes
    this.paymentForm.get('paymentMethod')?.valueChanges.subscribe((method) => {
      if (method) {
        this.toggleCardValidators(method);
      }
    });

  }
  ngOnDestroy() {
    //this.disconnectSocket();
   }
  
   // Initializes socket connection
   initializeSocketConnection() {
    this.socket$ = webSocket('wss://localhost:7092/api/Transactions/MakePayment');
    //this.websocketService.connectSocket('message');
   }
  
  //  // Receives response from socket connection 
  //  receiveSocketResponse() {
  //   this.websocketService.receiveStatus()
  //     .subscribe((receivedMessage: any) => {
  //       this.formSubmitting.set(false);
  //    console.log(receivedMessage);
  //   });
  //  }
  
  //  // Disconnects socket connection
  //  disconnectSocket() {
  //   this.websocketService.disconnectSocket();
  //  }
  private toggleCardValidators(method: string): void {
    const cardInfoGroup = this.paymentForm.get('cardInfo') as FormGroup;
    const bankInfoGroup = this.paymentForm.get('bankInfo') as FormGroup;

    if (method === 'card') {
      cardInfoGroup.get('cardNumber')?.setValidators([
        Validators.required,
        Validators.minLength(16),
        Validators.maxLength(16)
      ]);
      cardInfoGroup.get('expiryDate')?.setValidators([Validators.required]);
      cardInfoGroup.get('cvv')?.setValidators([
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(4)
      ]);
      bankInfoGroup.get('accountNumber')?.clearValidators();
      bankInfoGroup.get('routingNumber')?.clearValidators();

    } else {
      cardInfoGroup.get('cardNumber')?.clearValidators();
      cardInfoGroup.get('expiryDate')?.clearValidators();
      cardInfoGroup.get('cvv')?.clearValidators();

      bankInfoGroup.get('accountNumber')?.setValidators([
        Validators.required
      ]);
      bankInfoGroup.get('routingNumber')?.setValidators([
        Validators.required
      ]);
    }

    cardInfoGroup.get('cardNumber')?.updateValueAndValidity();
    cardInfoGroup.get('expiryDate')?.updateValueAndValidity();
    cardInfoGroup.get('cvv')?.updateValueAndValidity();
    bankInfoGroup.get('accountNumber')?.updateValueAndValidity();
    bankInfoGroup.get('routingNumber')?.updateValueAndValidity();
  }

  onPaymentSubmit() {
    if (this.paymentForm.valid) {
      this.formSubmitting.set(true);
      const paymentData = this.paymentForm.value;
      this.socket$.next(paymentData);
      //this.websocketService.postData(paymentData);

      this.socket$.subscribe(
        (response) => {
          //this.formSubmitting.set(false);
          this.waitingResponse.set(true);
          console.log('Payment response:', response);
        },
        (err) => {
          this.formSubmitting.set(false);
          console.error('WebSocket error:', err);
        }
      );
    } else {
      console.error('Payment form is invalid');
    }
  }
}

