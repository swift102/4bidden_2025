import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserProfile, UpdateProfileDto, ChangePasswordDto, ApiResponse } from '../models/user.model';


@Injectable({
  providedIn: 'root'
})
export class ProfileService {
private apiUrl = 'https://your-api-url.com/api/Profile'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken'); // Adjust based on your auth implementation
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getProfile(): Observable<ApiResponse<UserProfile>> {
    return this.http.get<ApiResponse<UserProfile>>(`${this.apiUrl}/GetProfile`, {
      headers: this.getHeaders()
    });
  }

  updateProfile(profile: UpdateProfileDto): Observable<ApiResponse<UserProfile>> {
    return this.http.put<ApiResponse<UserProfile>>(`${this.apiUrl}/UpdateProfile`, profile, {
      headers: this.getHeaders()
    });
  }

  changePassword(passwordData: ChangePasswordDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/change-password`, passwordData, {
      headers: this.getHeaders()
    });
  }

  deleteAccount(): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/DeleteAccount`, {
      headers: this.getHeaders()
    });
  }
}