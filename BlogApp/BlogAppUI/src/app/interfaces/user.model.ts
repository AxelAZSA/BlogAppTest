export interface User {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
  }
  
  export interface RegisterDTO {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  }
  
  export interface LoginDto {
    email: string;
    password: string;
  }
  
  export interface RefreshRequest {
    refreshToken: string;
  }
  
  export interface AuthResponse {
    token: string;
    refreshToken: string;
  }
  
  export interface ErrorResponse {
    errors: string[];
  }
  