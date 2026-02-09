import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UsersService } from '../../../services/users.service';
import { RegisterRequest, ApiResponse, AuthenticationResponse, GenderOptions } from '../../../models/user.model';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent {
  private usersService = inject(UsersService);
  private router = inject(Router);

  registerRequest = signal<RegisterRequest>({
    email: '',
    password: '',
    personName: '',
    gender: GenderOptions.Other
  });
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);
  genders = Object.values(GenderOptions);

  register() {
    if (!this.registerRequest().email || !this.registerRequest().password || !this.registerRequest().personName) {
      this.error.set('Please fill in all required fields');
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.success.set(null);

    this.usersService.register(this.registerRequest()).subscribe({
      next: (response: ApiResponse<AuthenticationResponse>) => {
        if (response.success && response.data) {
          this.success.set('Registration successful!');
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
          this.error.set(response.message || 'Registration failed');
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Registration failed. Please try again.');
        this.loading.set(false);
        console.error('Error registering:', err);
      }
    });
  }

  updateRegisterEmail(value: string) {
    this.registerRequest.update(r => ({ ...r, email: value }));
  }

  updateRegisterPassword(value: string) {
    this.registerRequest.update(r => ({ ...r, password: value }));
  }

  updatePersonName(value: string) {
    this.registerRequest.update(r => ({ ...r, personName: value }));
  }

  updateGender(value: GenderOptions) {
    this.registerRequest.update(r => ({ ...r, gender: value }));
  }
}
