export interface UserProfile {
  id: string;
  Name: string;
  email: string;
  userName: string;
  notificationsEnabled: boolean;
  theme: string;
  notificationThreshold: number;
  timeZone: string;
  emailNotifications: boolean;
  smsNotifications: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export interface UpdateProfileDto {
  name: string;
  email: string;
  notificationsEnabled: boolean;
  theme: string;
  notificationThreshold: number;
  timeZone: string;
  emailNotifications: boolean;
  smsNotifications: boolean;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
}