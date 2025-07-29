import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserProfile, UpdateProfileDto, ChangePasswordDto, ApiResponse } from '../models/user.model';


@Injectable({
  providedIn: 'root'
})
export class ProfileService {
private apiUrl = 'https://localhost:7197/api/Profile'; // Replace with your API URL

 constructor(private http: HttpClient) {}

  // Remove the getHeaders method and use simple requests
  getProfile(): Observable<ApiResponse<UserProfile>> {
    return this.http.get<ApiResponse<UserProfile>>(`${this.apiUrl}/GetProfile`);
  }

  updateProfile(profile: UpdateProfileDto): Observable<ApiResponse<UserProfile>> {
    return this.http.put<ApiResponse<UserProfile>>(`${this.apiUrl}/UpdateProfile`, profile);
  }

  changePassword(passwordData: ChangePasswordDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/change-password`, passwordData);
  }

  deleteAccount(): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/DeleteAccount`);
  }
}