import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface UserProfile {
  customerId?: string;
  customerName: string;
  phoneNumber?: string;
  address?: string;
  email?: string;
}

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class ProfileComponent implements OnInit {
  profile: UserProfile = {
    customerName: '',
    email: '',
    phoneNumber: '',
    address: ''
  };
  isEditing = false;
  isLoading = true;
  profileExists = false;
  userId = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.userId = sessionStorage.getItem('UserId') || '';

    console.log('Loading profile for user:', this.userId);

    if (!this.userId) {
      console.error('No user ID found - user not logged in');
      alert('Please login to view your profile');
      this.isLoading = false;
      return;
    }

    const getUrl = `https://localhost:5002/api/Customer/${this.userId}`;

    this.http.get<any>(getUrl).subscribe({
      next: (response) => {
        console.log('API Response:', response);
        if (response.isSuccess && response.result) {
          this.profile = {
            customerId: response.result.customerId,
            customerName: response.result.customerName,
            phoneNumber: response.result.phoneNumber,
            address: response.result.address,
            email: response.result.email || sessionStorage.getItem('UserEmail') || ''
          };
          this.profileExists = true;
        } else {
          this.initializeEmptyProfile();
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading profile:', error);
        this.initializeEmptyProfile();
        this.isLoading = false;
      }
    });
  }

  initializeEmptyProfile(): void {
    this.profileExists = false;
    this.profile = {
      customerId: this.userId,
      customerName: '',
      phoneNumber: '',
      address: '',
      email: sessionStorage.getItem('UserEmail') || ''
    };
    this.isEditing = true;
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing && !this.profileExists) {
      this.loadProfile();
    }
  }

  saveProfile(): void {
    if (!confirm('Are you sure you want to save these changes?')) {
      return;
    }
    
    this.isLoading = true;
    
    const url = `https://localhost:5002/api/Customer`;
    const request = this.profileExists
      ? this.http.put(url, this.profile)
      : this.http.post(url, this.profile);

    request.subscribe({
      next: (response: any) => {
        if (response?.isSuccess) {
          alert('Profile saved successfully');
          this.profileExists = true;
          this.isEditing = false;
          this.loadProfile();
        } else {
          alert(response?.message || 'Profile saved but no success status returned');
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error saving profile:', error);
        alert(error.error?.message || 'Failed to save profile. Please try again.');
        this.isLoading = false;
      }
    });
  }
}