export interface UserDTO {
  userId: string;
  email?: string;
  personName?: string;
  gender: string;
}

export interface RegisterRequest {
  email?: string;
  password?: string;
  personName?: string;
  gender: GenderOptions;
}

export interface LoginRequest {
  email?: string;
  password?: string;
}

export interface AuthenticationResponse {
  token?: string;
  email?: string;
  personName?: string;
  userId?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T | null;
}

export enum GenderOptions {
  Male = 'Male',
  Female = 'Female',
  Other = 'Other'
}
