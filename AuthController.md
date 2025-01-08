# AuthController API Documentation

## Overview
The `AuthController` handles user authentication-related operations such as registration, login, token refresh, and logout.

## Endpoints

### Register
- **URL:** `POST /api/auth/register`
- **Description:** Registers a new user.
- **Request Body:** 
- {
  "email": "bilal@example.com",      
  "password": "Bilal@1",      
  "confirmPassword": "Bilal@1"
}
- **Responses:**
  - `200 OK`: {
  "success": true,
  "message": "User registered successfully."
}
- `400 Bad Request`: {
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",                       
  "title": "One or more validation errors occurred.",                                 
  "status": 400,                                                                          
  "errors": {
    "Email": [
      "The Email field is not a valid e-mail address."
    ]                          
  },                                                                                                           
  "traceId": "00-a2e3d4680ec081d6f34faca6dfd75e2a-8b59315593b3a145-00"                                                           
}
### Login
- **URL:** `POST /api/auth/login`
- **Description:** Logs in a user.
- **Request Body:**  {
  "email": "bilal@example.com",                                      
  "password": "Bilal@1"                                   
}
- **Responses:**
  - `200 OK`: {
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiN2I3YjkzOC04ZTAzLTRhNDgtODQzMi05NTgzYmE2MDhkNTkiLCJlbWFpbCI6ImJpbGFsQGV4YW1wbGUuY29tIiwianRpIjoiNTEzNDkxZjEtZmVhOS00OGFhLTg4NTYtNTE1MDBmMjc5NDEyIiwiZXhwIjoxNzM1NDUyNzE1fQ.6QXHlyWnbwaIIw_s6am93sFo7BbD0Skz_tArhibU1nQ",
  "accessTokenExpiry": "2024-12-29T06:11:55.9562503Z",                                                                                                                                                        
  "refreshToken": "8efff244-d927-4c8b-9459-236c82115cf2",                                                
  "refreshTokenExpiry": "2025-01-05T05:41:55.915871Z",                                          
  "message": "Login successful."                                                                  
}
- `401 Unauthorized`:{
  "success": false,                              
  "message": "Invalid email or password."
}
### Refresh Token
- **URL:** `POST /api/auth/refresh`
- **Description:** Refreshes the access token using a refresh token.
- **Request Body:**  {
  "token": "dc23590f-c14e-43c3-9729-80255ecdbf2c"
}
- **Responses:**
  - `200 OK`: {
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiN2I3YjkzOC04ZTAzLTRhNDgtODQzMi05NTgzYmE2MDhkNTkiLCJlbWFpbCI6ImJpbGFsQGV4YW1wbGUuY29tIiwianRpIjoiYzI3N2RlNzYtMWI1Mi00Y2ZhLWFlY2UtMTk3OGQwMzUwMDBjIiwiZXhwIjoxNzM1NDUzMDcyfQ.1EMntkXR6h-oA6G-xOnOmgBQXTJw4zwdD25fp5YHNRc",
  "accessTokenExpiry": "2024-12-29T06:17:52.4554024Z",                
  "refreshToken": "f7982723-028f-470f-9fa4-1de74cf60a5f",
  "refreshTokenExpiry": "2025-01-05T05:47:52.4310714Z",
  "message": "Token refreshed successfully."
}
- `401 Unauthorized`: {
  "success": false,
  "message": "Invalid or expired refresh token."
}
### Logout
- **URL:** `POST /api/auth/logout`
- **Description:** Logs out a user by invalidating the refresh token.
- **Request Body:** {
  "refreshToken": "dc23590f-c14e-43c3-9729-80255ecdbf2c"}
- **Responses:**
  - `200 OK`: {
  "message": "Successfully logged out."
}
- `400 Bad Request`: {
  "message": "Invalid refresh token."
}
## Data Transfer Objects (DTOs)

### RegisterDto
### LoginDto
### RefreshTokenDto
### LogoutRequestDto
### AuthResponseDto
