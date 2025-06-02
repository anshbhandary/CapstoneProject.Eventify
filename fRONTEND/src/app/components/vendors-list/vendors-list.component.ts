import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Modal } from 'bootstrap';
import { CommonModule } from '@angular/common';

interface Vendor {
  username: string;
  vendorId: string;
  vendorEmail: string;
  vendorPhno: string;
}

interface Package {
  packageName: string;
  description: string;
  price: number;
}

interface ApiResponse<T> {
  result: T;
  isSuccess: boolean;
  message: string;
}

@Component({
  selector: 'app-vendors',
  templateUrl: './vendors-list.component.html',
  styleUrls: ['./vendors-list.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class VendorsComponent implements OnInit {
  vendors: Vendor[] = [];
  packages: Package[] = [];
  isLoading = true;
  packagesLoading = false;
  selectedVendor: Vendor | null = null;
  private packagesModal: Modal | undefined;

  private readonly VENDORS_API = 'https://localhost:5001/api/vendor/GetAllVendors';
  private readonly PACKAGES_API = 'https://localhost:5001/api/vendor/vendor-package-requests/by-vendor/'; // Changed endpoint name

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchVendors();
  }

  fetchVendors(): void {
    this.isLoading = true;
    this.http.get<ApiResponse<Vendor[]>>(this.VENDORS_API)
      .subscribe({
        next: (response) => this.handleVendorsResponse(response),
        error: (error) => this.handleVendorsError(error)
      });
  }

  private handleVendorsResponse(response: ApiResponse<Vendor[]>): void {
    if (response.isSuccess && response.result) {
      this.vendors = response.result;
    } else {
      console.error('Failed to fetch vendors:', response.message);
      alert(response.message || 'Failed to load vendors');
    }
    this.isLoading = false;
  }

  private handleVendorsError(error: HttpErrorResponse): void {
    console.error('Error fetching vendors:', error);
    this.isLoading = false;
    alert('Failed to load vendors. Please try again later.');
  }

  viewPackages(vendorId: string): void {
    const vendor = this.vendors.find(v => v.vendorId === vendorId);
    if (!vendor) return;

    this.selectedVendor = vendor;
    this.fetchPackages(vendorId);
    
    const modalElement = document.getElementById('packagesModal');
    if (modalElement) {
      this.packagesModal = new Modal(modalElement);
      this.packagesModal.show();
    }
  }

  fetchPackages(vendorId: string): void {
    this.packagesLoading = true;
    this.packages = [];
  
    const vendorIdStr = String(vendorId); // ensure it's a string
    this.http.get<ApiResponse<Package[]>>(
      `${this.PACKAGES_API}${vendorIdStr}`
    ).subscribe({
      next: (response) => this.handlePackagesResponse(response),
      error: (error) => this.handlePackagesError(error)
    });
  }
  

  private handlePackagesResponse(response: ApiResponse<Package[]>): void {
    console.log(response);
    if (response.isSuccess && response.result) {
      this.packages = Array.isArray(response.result) 
        ? response.result 
        : [response.result];
    } else {
      console.error('Failed to fetch packages:', response.message);
      alert(response.message || 'No packages available');
    }
    this.packagesLoading = false;
  }

  private handlePackagesError(error: HttpErrorResponse): void {
    console.error('Error fetching packages:', error);
    this.packagesLoading = false;
    
    let errorMessage = 'Failed to load packages. Please try again later.';
    if (error.error?.message) {
      errorMessage = error.error.message;
    }
    
    alert(errorMessage);
  }

  selectPackage(pkg: Package): void {
    if (!this.selectedVendor) return;
    
    const subject = `Booking Inquiry: ${pkg.packageName} Package`;
    const body = `Dear ${this.selectedVendor.username},\n\n` +
                 `I would like to inquire about booking your ${pkg.packageName} package.\n\n` +
                 `Package Details:\n` +
                 `- Name: ${pkg.packageName}\n` +
                 `- Description: ${pkg.description}\n` +
                 `- Price: ₹${pkg.price.toLocaleString('en-IN')}\n\n` +
                 `Please let me know about availability and next steps.\n\n` +
                 `Best regards,\n[Your Name]`;
    
    // Encode the subject and body for URL
    const encodedSubject = encodeURIComponent(subject);
    const encodedBody = encodeURIComponent(body);
    
    // Open Gmail compose window
    window.open(`https://mail.google.com/mail/?view=cm&fs=1&to=${this.selectedVendor.vendorEmail}&su=${encodedSubject}&body=${encodedBody}`, '_blank');
    this.packagesModal?.hide();
  }
}