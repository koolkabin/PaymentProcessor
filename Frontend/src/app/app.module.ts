import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { PaymentFormComponent } from './components/payment-form/payment-form.component';
import { TransactionHistoryComponent } from './components/transaction-history/transaction-history.component';

@NgModule({
  declarations: [
    AppComponent,
    PaymentFormComponent,
    TransactionHistoryComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
