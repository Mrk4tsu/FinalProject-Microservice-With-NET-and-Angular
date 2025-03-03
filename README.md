# Microservices Architecture Project for the Graduation Thesis at Nha Trang University.
<div align="center">
  <img src="https://github.com/user-attachments/assets/604e4196-b16c-4b0a-b3f7-e1a029f23e43"/>
</div>

## 📖 Overview  
This project is designed using a **microservices architecture** to provide a scalable and maintainable system. It is developed as part of the **graduation thesis at Nha Trang University**. 

DEMO: [mrkatsu.io.vn](https://mrkatsu.io.vn)
## 🚀 Features 
- **User Authentication & Authorization** (JWT, Firebase Authentication)
- **Service-to-Service Communication** (REST API / gRPC)  
- **Database Management** (MongoDB, SQL Server, Redis) 
- **File Uploads** (MEGA Drive API, Cloudinary) 
- **Logging & Monitoring** (Serilog, ELK Stack) 
- **Caching Mechanism** (Redis)  
## 🏗️ Project Architecture  
The system consists of multiple independent services, including: 
1. **[API Gateway](https://github.com/Mrk4tsu/FinalProject-Microservice-With-NET-and-Angular/tree/master/Server/FN.APIGateway#danh-s%C3%A1ch-x%E1%BB%AD-l%C3%BD-c%C3%A1c-api-gateway)** – Handles request routing, authentication, and rate limiting.  
2. **User Service** – Manages user registration, authentication, and profiles. 
3. **Product Service** – Handles product listings, categories, and inventory. 
4. **Order Service** – Manages orders, transactions, and payments.
5. **Notification Service** – Sends emails, SMS, and in-app notifications.
6. **Logging Service** – Centralized logging and monitoring system. 
7. **Extension** - Other services can be added as needed.
## 🛠️ Technologies Used 
- **Backend**: ASP.NET Core 8
- **Frontend**: Angular 19
- **Database**: MySQL, MongoDB, Redis 
- **Authentication**: Firebase Authentication, JWT 
- **API Communication**: RESTful API / gRPC 
- **Cloud Services**: MEGA Drive API, Cloudinary
## 🎯 Setup Instructions
### 📌 Prerequisites 
### 🛠️ Installation  
#### Setup Environment && Tools
1. Setup Environment Net Core 8 SDK
2. Setup MongoDB (localhost) or MongoDB Atlas (Online)
3. Setup Redis CLI (localhost) or Resdis Server(Online)
4. Setup MySQL (localhost) or Online
5. Docker (Option) deploy
6. Need a Cloud Server (Google Cloud, ASW, Azure, Cloudinary,...)
7. Firebase