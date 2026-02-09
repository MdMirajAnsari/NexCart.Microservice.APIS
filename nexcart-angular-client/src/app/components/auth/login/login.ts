import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UsersService } from '../../../services/users.service';
import { LoginRequest, ApiResponse, AuthenticationResponse } from '../../../models/user.model';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent {
  private usersService = inject(UsersService);
  private router = inject(Router);

  loginRequest = signal<LoginRequest>({
    email: '',
    password: ''
  });
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  login() {
    if (!this.loginRequest().email || !this.loginRequest().password) {
      this.error.set('Please enter both email and password');
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.success.set(null);

    this.usersService.login(this.loginRequest()).subscribe({
      next: (response: ApiResponse<AuthenticationResponse>) => {
        if (response.success && response.data) {
          this.success.set('Login successful!');
          // Store token if available
          if (response.data.token) {
            localStorage.setItem('token', response.data.token);
          }
          // Store user info
          if (response.data.userId) {
            localStorage.setItem('userId', response.data.userId);
          }
          setTimeout(() => {
            this.router.navigate(['/products']);
          }, 1500);
        } else {
          this.error.set(response.message || 'Login failed');
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Login failed. Please check your credentials.');
        this.loading.set(false);
        console.error('Error logging in:', err);
      }
    });
  }

  updateEmail(value: string) {
    this.loginRequest.update(r => ({ ...r, email: value }));
  }

  updatePassword(value: string) {
    this.loginRequest.update(r => ({ ...r, password: value }));
  }
}
