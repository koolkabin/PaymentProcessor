# Payment Gateway Module

## Overview
This project implements a payment gateway module using event-driven architecture. It consists of a .NET Core backend, an Angular frontend, and RabbitMQ for asynchronous communication. The system simulates payment processing with a mock third-party API and provides features like transaction management and real-time updates.

---

## Features

### Backend (C#/.NET Core)
- **Payment Processing API:** Accepts payment requests and simulates third-party payment processing.
- **Event-Driven Architecture:** Publishes and consumes events for transaction status updates using RabbitMQ.
- **Transaction Management:** Stores transaction details in a relational database.
- **Event Monitoring:** Provides logs of recent events.
- **Security:** Implements OWASP principles and JWT-based authentication.

### Frontend (Angular)
- **Payment Form:** Allows users to input payment details.
- **Transaction History:** Displays past transactions with real-time updates using WebSocket or polling.

---

## System Requirements

- **Backend:** .NET Core 6 or higher
- **Frontend:** Angular 15 or higher
- **Database:** PostgreSQL (or other relational DBs like MySQL, SQL Server)
- **Messaging:** RabbitMQ
- **Docker:** Docker and Docker Compose for deployment

---

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/your-repo/payment-gateway.git
cd payment-gateway
```

### 2. Backend Setup

1. **Navigate to the Backend Directory:**
```bash
cd Backend
```

2. **Restore Dependencies:**
```bash
dotnet restore
```

3. **Apply Migrations and Seed Data:**
```bash
dotnet ef database update
```

4. **Run the Backend Service:**
```bash
dotnet run
```

5. **Environment Variables:**
   - Set the following in `appsettings.json` or environment variables:
     - RabbitMQ connection string
     - JWT secret key
     - Database connection string

### 3. Frontend Setup

1. **Navigate to the Frontend Directory:**
```bash
cd Frontend
```

2. **Install Dependencies:**
```bash
npm install
```

3. **Run the Development Server:**
```bash
ng serve
```

### 4. RabbitMQ Setup

1. **Start RabbitMQ with Docker Compose:**
```bash
cd RabbitMQ
docker-compose up -d
```

2. **Access RabbitMQ Management Console:**
   - URL: `http://localhost:15672`
   - Default credentials: `guest/guest`

### 5. Docker Deployment

1. **Build and Start Services:**
```bash
docker-compose up --build
```

2. **Access the Application:**
   - Backend API: `http://localhost:5000`
   - Frontend: `http://localhost:4200`

---

## API Endpoints

### Payment Processing
- **POST /payments/process**
  - Request Body:
    ```json
    {
      "amount": 100,
      "currency": "USD",
      "customer": {
        "name": "John Doe",
        "email": "john.doe@example.com"
      },
      "paymentMethod": "card",
      "cardDetails": {
        "number": "4111111111111111",
        "expiry": "12/25",
        "cvv": "123"
      }
    }
    ```

### Transaction History
- **GET /transactions**
  - Query Parameters: `?status=success&dateFrom=2023-01-01&dateTo=2023-12-31`

### Event Logs
- **GET /events/logs**

---

## Architecture

1. **Event-Driven:**
   - RabbitMQ is used to publish and consume events for asynchronous processing.

2. **Database:**
   - Transaction details are stored in a relational database for consistency and querying.

3. **Frontend-Backend Communication:**
   - REST API endpoints are used for submitting payments and fetching transaction data.

---

## Optional Features

1. **Retry Logic:** Handles failed transactions and retries them based on a configured policy.
2. **Notification System:** Sends email notifications for successful transactions.

---

## Evaluation Criteria

1. **Event-Driven Design:** Efficient use of RabbitMQ for asynchronous processing.
2. **Integration & Testing:** Smooth interaction with the mock API and well-defined API endpoints.
3. **Code Quality & Security:** Clean, maintainable code adhering to security standards.
4. **User Experience:** Intuitive and responsive frontend.
5. **Documentation:** Clear instructions for setup and testing.

---

## Contact
For any questions or support, please contact Kabindra Bakey at koolkabin@live.com.

