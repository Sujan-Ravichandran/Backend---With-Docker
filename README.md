# Backend---With-Docker
<img width="4032" height="1008" alt="Pasted image" src="https://github.com/user-attachments/assets/5c86dee6-03ee-47e0-b080-061e663f8725" />
Doctor – Patient Appointment Booking System - Microservices Project Completion Summary 

Project Overview 

Developed a Doctor-Patient Appointment Booking System using a Microservices Architecture with ASP.NET Core, PostgreSQL, Dapper, JWT Authentication, API Gateway, and Docker Compose. 

 

1. User Service 

Purpose 

Handles user management for: 

Patients 

Doctors 

Admins 

Features Implemented 

-->User Registration 

HTTP 

POST /api/auth/register 

-->User Login 

HTTP 

POST /api/auth/login 

-->Password Hashing 

Used: 

Plain Text 

BCrypt 

-->JWT Token Generation 

Generated token after successful login. 

-->Get User By Id 

HTTP 

GET /api/auth/{id} 

-->Get All Users 

HTTP 

GET /api/auth 

-->Update User 

HTTP 

PUT /api/auth/{id} 

-->Delete User 

HTTP 

DELETE /api/auth/{id} 

-->Role Management 

Plain Text 

Patient 

Doctor 

Admin 

 

2. Doctor Service 

Purpose 

Maintains doctor profiles separately from users. 

Features Implemented 

-->Create Doctor 

HTTP 

POST /api/doctors 

-->Get All Doctors 

HTTP 

GET /api/doctors 

-->Pagination 

HTTP 

GET /api/doctors?pageNumber=1&pageSize=10 

-->Get Doctor By Id 

HTTP 

GET /api/doctors/{id} 

-->Update Doctor 

HTTP 

PUT /api/doctors/{id} 

-->Delete Doctor 

HTTP 

DELETE /api/doctors/{id} 

 

Doctor Service Validations 

User Validation 

Before creating a doctor profile: 

Plain Text 

Doctor Service 

| 

▼ 

User Service 

Checks: 

-->User exists 

 

Role Validation 

Ensures: 

Plain Text 

Role = Doctor 

Example: 

Plain Text 

Patient UserId 

| 

▼ 

Doctor Creation 

| 

▼ 

Rejected 

 

Duplicate Profile Validation 

Prevents: 

Plain Text 

UserId = 5 

from creating multiple doctor profiles. 

 

3. Appointment Service 

Purpose 

Appointment booking and management. 

Features Implemented 

-->Create Appointment 

-->Get Appointment 

-->Update Appointment 

-->Delete Appointment 

 

Appointment Validations 

Patient Validation 

Appointment Service validates: 

Plain Text 

Patient Exists? 

Role = Patient? 

through User Service. 

 

Doctor Validation 

Appointment Service validates: 

Plain Text 

Doctor Exists? 

through Doctor Service. 

 

Slot Validation 

Prevents: 

Plain Text 

Same Doctor 

Same Date 

Same Time 

from being booked twice. 

 

Availability Validation 

Checks doctor's availability before booking. 

 

4. Database Layer 

Used: 

Plain Text 

PostgreSQL 

and 

Plain Text 

Dapper ORM 

Implemented: 

-->Repository Pattern 

-->Dependency Injection 

-->Separate Database Contexts 

 

5. JWT Authentication 

Implemented JWT Token Security. 

Login Flow 

Plain Text 

User Login 

| 

▼ 

JWT Generated 

| 

▼ 

Token Returned 

JWT Contains 

Plain Text 

UserId 

Email 

Role 

Authorization Header 

HTTP 

Authorization: Bearer <token> 

 

6. API Authorization 

Protected APIs using: 

C# 

[Authorize] 

Successfully validated JWT tokens across services. 

 

7. Service-to-Service Communication 

Implemented HTTP Communication between services. 

Doctor Service → User Service 

Plain Text 

Validate User 

Validate Doctor Role 

Appointment Service → User Service 

Plain Text 

Validate Patient 

Appointment Service → Doctor Service 

Plain Text 

Validate Doctor 

Validate Availability 

 

8. API Gateway 

Implemented: 

Plain Text 

YARP Reverse Proxy 

Gateway Routes 

User Service 

Plain Text 

/users/* 

Doctor Service 

Plain Text 

/doctors/* 

Appointment Service 

Plain Text 

/appointments/* 

Request Flow 

Plain Text 

Client 

| 

▼ 

API Gateway 

| 

├── User Service 

├── Doctor Service 

└── Appointment Service 

Successfully configured: 

✅ Route Mapping 

✅ Path Prefix Removal 

✅ Gateway Testing 

 

9. Dockerization 

Created Dockerfiles for: 

✅ User Service 

✅ Doctor Service 

✅ Appointment Service 

✅ API Gateway 

 

10. Docker Compose 

Configured: 

YAML 

docker-compose.yml 

to run: 

Plain Text 

User Service 

Doctor Service 

Appointment Service 

API Gateway 

through a single command: 

Shell 

docker compose up --build 

Successfully resolved: 

✅ Container Networking 

✅ Service Connectivity 

✅ Local PostgreSQL Access 

 

Final Architecture 

Plain Text 

Client 

| 

▼ 

  

API Gateway 

| 

---------------------------------- 

| | | 

▼ ▼ ▼ 

  

User Service Doctor Service Appointment Service 

  

| | | 

---------------------------------- 

| 

▼ 

  

PostgreSQL 

 

Technologies Used 

Plain Text 

ASP.NET Core Web API 

C# 

PostgreSQL 

Dapper 

JWT Authentication 

YARP API Gateway 

Docker 

Docker Compose 

BCrypt 

REST APIs 

Microservices Architecture 

 

Key Microservice Concepts Demonstrated 

✅ Independent Services 

✅ Database Per Service 

✅ Service-to-Service Communication 

✅ JWT Authentication 

✅ API Gateway Pattern 

✅ Repository Pattern 

✅ Dependency Injection 

✅ Docker Containerization 

✅ Docker Compose Orchestration 

✅ Validation Across Services 

✅ Role-Based Architecture 

 
