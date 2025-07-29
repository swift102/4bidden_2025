import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProfileService } from '../../services/profile.service';
import { UserProfile, UpdateProfileDto, ChangePasswordDto } from '../../models/user.model';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { DatePipe,  CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule, DatePipe, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  passwordForm!: FormGroup;
  userProfile: UserProfile | null = null;
  
  loading = false;
  profileUpdateSuccess = false;
  passwordChangeSuccess = false;
  errorMessage = '';
  
  activeTab = 'profile'; // 'profile' or 'password'
  
  themes = [
    { value: 'light', label: 'Light Theme' },
    { value: 'dark', label: 'Dark Theme' },
    { value: 'auto', label: 'Auto (System)' }
  ];

  timeZones = [
    { value: 'UTC', label: 'UTC' },
    { value: 'America/New_York', label: 'Eastern Time' },
    { value: 'America/Chicago', label: 'Central Time' },
    { value: 'America/Denver', label: 'Mountain Time' },
    { value: 'America/Los_Angeles', label: 'Pacific Time' },
    { value: 'Europe/London', label: 'London' },
    { value: 'Europe/Paris', label: 'Paris' },
    { value: 'Asia/Tokyo', label: 'Tokyo' },
    { value: 'Australia/Sydney', label: 'Sydney' }
  ];

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService
  ) {
    this.initializeForms();
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  initializeForms(): void {
    this.profileForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      notificationsEnabled: [true],
      theme: ['light', Validators.required],
      notificationThreshold: [5, [Validators.required, Validators.min(1), Validators.max(100)]],
      timeZone: ['UTC'],
      emailNotifications: [true],
      smsNotifications: [false]
    });

    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const newPassword = form.get('newPassword');
    const confirmPassword = form.get('confirmPassword');
    
    if (newPassword && confirmPassword && newPassword.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    
    if (confirmPassword?.hasError('passwordMismatch')) {
      delete confirmPassword.errors!['passwordMismatch'];
      if (Object.keys(confirmPassword.errors!).length === 0) {
        confirmPassword.setErrors(null);
      }
    }
    
    return null;
  }

  loadProfile(): void {
    this.loading = true;
    this.errorMessage = '';
    
    this.profileService.getProfile().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.userProfile = response.data;
          this.populateForm();
        } else {
          this.errorMessage = response.message || 'Failed to load profile';
        }
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load profile. Please try again.';
        this.loading = false;
        console.error('Profile load error:', error);
      }
    });
  }

  populateForm(): void {
    if (this.userProfile) {
      this.profileForm.patchValue({
        name: this.userProfile.Name,
        email: this.userProfile.email,
        notificationsEnabled: this.userProfile.notificationsEnabled,
        theme: this.userProfile.theme,
        notificationThreshold: this.userProfile.notificationThreshold,
        timeZone: this.userProfile.timeZone,
        emailNotifications: this.userProfile.emailNotifications,
        smsNotifications: this.userProfile.smsNotifications
      });
    }
  }

  onProfileSubmit(): void {
    if (this.profileForm.valid) {
      this.loading = true;
      this.profileUpdateSuccess = false;
      this.errorMessage = '';

      const updateData: UpdateProfileDto = this.profileForm.value;

      this.profileService.updateProfile(updateData).subscribe({
        next: (response) => {
          if (response.success && response.data) {
            this.userProfile = response.data;
            this.profileUpdateSuccess = true;
            setTimeout(() => this.profileUpdateSuccess = false, 3000);
          } else {
            this.errorMessage = response.message || 'Failed to update profile';
          }
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to update profile. Please try again.';
          this.loading = false;
          console.error('Profile update error:', error);
        }
      });
    } else {
      this.markFormGroupTouched(this.profileForm);
    }
  }

  onPasswordSubmit(): void {
    if (this.passwordForm.valid) {
      this.loading = true;
      this.passwordChangeSuccess = false;
      this.errorMessage = '';

      const passwordData: ChangePasswordDto = this.passwordForm.value;

      this.profileService.changePassword(passwordData).subscribe({
        next: (response) => {
          if (response.success) {
            this.passwordChangeSuccess = true;
            this.passwordForm.reset();
            setTimeout(() => this.passwordChangeSuccess = false, 3000);
          } else {
            this.errorMessage = response.message || 'Failed to change password';
          }
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to change password. Please try again.';
          this.loading = false;
          console.error('Password change error:', error);
        }
      });
    } else {
      this.markFormGroupTouched(this.passwordForm);
    }
  }

  onDeleteAccount(): void {
    if (confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
      this.loading = true;
      
      this.profileService.deleteAccount().subscribe({
        next: (response) => {
          if (response.success) {
            alert('Account deleted successfully');
            // Redirect to login or home page
            // this.router.navigate(['/login']);
          } else {
            this.errorMessage = response.message || 'Failed to delete account';
          }
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to delete account. Please try again.';
          this.loading = false;
          console.error('Account deletion error:', error);
        }
      });
    }
  }

  switchTab(tab: string): void {
    this.activeTab = tab;
    this.errorMessage = '';
    this.profileUpdateSuccess = false;
    this.passwordChangeSuccess = false;
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(form: FormGroup, fieldName: string): string {
    const field = form.get(fieldName);
    if (field && field.touched && field.errors) {
      if (field.errors['required']) return `${fieldName} is required`;
      if (field.errors['email']) return 'Please enter a valid email';
      if (field.errors['minlength']) return `Minimum length is ${field.errors['minlength'].requiredLength}`;
      if (field.errors['maxlength']) return `Maximum length is ${field.errors['maxlength'].requiredLength}`;
      if (field.errors['min']) return `Minimum value is ${field.errors['min'].min}`;
      if (field.errors['max']) return `Maximum value is ${field.errors['max'].max}`;
      if (field.errors['passwordMismatch']) return 'Passwords do not match';
    }
    return '';
  }
}
