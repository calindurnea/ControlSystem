# Standardized Control System

System based on a microservices architecture that enables for a standardized communication between operations team from Manufacturing and Warehouse facilities and AMR (Autonomous Mobile Robot) or AGV (Automated Guided Vehicle) providers.

https://miro.com/app/board/uXjVNL0TE5g=/?share_link_id=31486949280

# Installation
## Dependencies
- Docker

## Running the solution
- Clone the repository
- Navigate inside of `ControlSystem` (`cd ControlSystem`)
- Run `docker compose up -d`

# Usage
- The exposed url is `localhost:5002` (swagger is available at `http://localhost:5002/swagger/index.html`)
- Functional endpoints are `POST http://localhost:5002/api/Orders` and `GET http://localhost:5002/api/Orders/<id>`

# Project structure (main parts only)
    .
    ├── integration-service
    │    ├── IntegrationService.Worker
    │    ├── IntegrationService.AgvProviderFactoryUs
    │    ├── IntegrationService.AgvProviderFactoryHu
    │    └── ...
    │
    ├── order-service
    │    ├── OrderService.Api
    │    ├── OrderService.Application
    │    ├── OrderService.Domain
    │    ├── OrderService.Integration.Tests
    │    ├── OrderService.OrderManager
    │    └── OrderService.Persistence
    │
    └── libraries
        └── Libraries.Common
            └── Events
# Message flow diagram
![image](https://github.com/calindurnea/ControlSystem/assets/17986810/994dc2c3-bbee-4987-b21f-c3140212eae2)

