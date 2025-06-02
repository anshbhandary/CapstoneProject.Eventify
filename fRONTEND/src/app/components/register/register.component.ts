import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class RegisterComponent {
  selectedRole: string = 'Customer';
  registerForm: FormGroup;

  http = inject(HttpClient);
  router = inject(Router);
  fb = inject(FormBuilder);
  toastr = inject(ToastrService);

  constructor() {
    this.registerForm = this.fb.group({
      name: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.pattern(/^[a-zA-Z0-9_]+$/)
      ]],
      email: ['', [
        Validators.required,
        Validators.email,
        Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|in|org|net|edu|gov|mil|co|info|biz|me|io|[a-zA-Z]{2,})$/)
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/)
      ]],
      phoneNumber: ['', [
        Validators.required,
        Validators.pattern(/^[0-9]{10}$/)
      ]]
    });
  }

  selectRole(role: string) {
    this.selectedRole = role;
  }

  onRegister() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.toastr.warning('Please fill all required fields correctly', 'Validation Error');
      return;
    }

    if (!this.selectedRole) {
      this.toastr.warning('Please select your role', 'Validation Error');
      return;
    }

    const formData = {
      ...this.registerForm.value,
      phoneNumber: String(this.registerForm.value.phoneNumber),
      role: this.selectedRole
    };

    const registerEndpoint = `https://localhost:5005/api/auth/register/${this.selectedRole.toLowerCase()}`;

    this.http.post(registerEndpoint, formData).subscribe({
      next: (res: any) => {
        this.toastr.success('Registration successful!', 'Success');
        
        // Call additional API based on role
        if (this.selectedRole === 'Customer') {
          this.createCustomer(res.result);
        } else if (this.selectedRole === 'Vendor') {
          this.createVendor(res.result);
        } else {
          this.router.navigateByUrl('/login');
        }
      },
      error: (err) => {
        const errorMessage = err.error?.message || 'Registration failed. Please try again.';
        this.toastr.error(errorMessage, 'Error');
        console.error('Registration error:', err);
      }
    });
  }

  private createCustomer(customerId: string) {
    const customerData = {
      customerId: customerId,
      customerName: this.registerForm.value.name,
      phoneNumber: String(this.registerForm.value.phoneNumber),
      address: this.registerForm.value.email 
    };

    this.http.post('https://localhost:5002/api/Customer', customerData).subscribe({
      next: () => {
        this.toastr.success('Customer profile created successfully!', 'Success');
        this.router.navigateByUrl('/login');
      },
      error: (err) => {
        this.toastr.warning('Registration succeeded but customer profile creation failed', 'Warning');
        console.error('Customer creation error:', err);
        this.router.navigateByUrl('/login');
      }
    });
  }

  private createVendor(vendorId: string) {
    const vendorData = {
      username: this.registerForm.value.name,
      vendorId: vendorId,
      vendorEmail: this.registerForm.value.email,
      vendorPhno: String(this.registerForm.value.phoneNumber)
    };

    this.http.post('https://localhost:5001/api/vendor/AddVendor', vendorData).subscribe({
      next: () => {
        this.toastr.success('Vendor profile created successfully!', 'Success');
        this.router.navigateByUrl('/login');
      },
      error: (err) => {
        this.toastr.warning('Registration succeeded but vendor profile creation failed', 'Warning');
        console.error('Vendor creation error:', err);
        this.router.navigateByUrl('/login');
      }
    });
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }
}