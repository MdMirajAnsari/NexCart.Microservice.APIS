import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsersService } from '../../services/users.service';
import { UserDTO, ApiResponse } from '../../models/user.model';

@Component({
  selector: 'app-users',
  imports: [CommonModule, FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsersComponent {
  private usersService = inject(UsersService);

  user = signal<UserDTO | null>(null);
  userId = signal<string>('');
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  getUser() {
    if (!this.userId().trim()) {
      alert('Please enter a User ID');
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.user.set(null);

    this.usersService.getUserById(this.userId()).subscribe({
      next: (response: ApiResponse<UserDTO>) => {
        if (response.success && response.data) {
          this.user.set(response.data);
        } else {
          this.error.set(response.message || 'User not found');
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load user. Please check if the API is running.');
        this.loading.set(false);
        console.error('Error loading user:', err);
      }
    });
  }
}
