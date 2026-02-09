import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, UserDTO, RegisterRequest, LoginRequest, AuthenticationResponse } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:9090/api'; // Users API base URL

  getUserById(userId: string): Observable<ApiResponse<UserDTO>> {
    return this.http.get<ApiResponse<UserDTO>>(`${this.apiUrl}/users/${userId}`);
  }

  register(registerRequest: RegisterRequest): Observable<ApiResponse<AuthenticationResponse>> {
    return this.http.post<ApiResponse<AuthenticationResponse>>(`${this.apiUrl}/auth/register`, registerRequest);
  }

  login(loginRequest: LoginRequest): Observable<ApiResponse<AuthenticationResponse>> {
    return this.http.post<ApiResponse<AuthenticationResponse>>(`${this.apiUrl}/auth/login`, loginRequest);
  }
}
