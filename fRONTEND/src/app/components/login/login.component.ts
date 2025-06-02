import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword: boolean = false;

  http = inject(HttpClient);
  router = inject(Router);
  fb = inject(FormBuilder);
  toastr = inject(ToastrService);
  authService = inject(AuthService);

  constructor() {
    this.loginForm = this.fb.group({
      username: ['', [
        Validators.required,
        Validators.email,
        Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|in|org|net|edu|gov|mil|co|info|biz|me|io|[a-zA-Z]{2,})$/)

      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/)
      ]],
      rememberMe: [false]
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onLogin() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      this.toastr.warning('Please fill all required fields correctly', 'Validation Error');
      return;
    }

    const loginPayload = {
      Username: this.loginForm.value.username,
      Password: this.loginForm.value.password
    };

    this.http.post("https://localhost:5005/api/auth/Login", loginPayload).subscribe({
      next: (res: any) => {
        if (res.result) {
          localStorage.setItem('loginUser', loginPayload.Username);
          sessionStorage.setItem('UserId', res.result.user.id);
          localStorage.setItem('myLogInToken', res.result.token);
      
          const userRoles = res.result.roles;
          if (userRoles && userRoles.length > 0) {
            const role = userRoles[0];
            this.authService.login(role); // 🔥 This triggers the navbar update
      
            // Redirect based on role
            if (role === 'Customer') {
              this.router.navigateByUrl('dashboard/c');
            } else if (role === 'Vendor') {
              this.router.navigateByUrl('dashboard/v');
            } else {
              this.router.navigateByUrl('dashboard/c');
            }
          }
      
          this.toastr.success("Login Successful!", "Success");
        } else {
          this.toastr.warning(res.message || "Invalid credentials", "Warning");
        }
      },      
      error: (err) => {
        console.error(err);
        const errorMessage = err.error?.message || "Login failed. Please try again.";
        this.toastr.error(errorMessage, "Error");
      }
    });
  }

  navigateToRegister() {
    this.router.navigate(['/register']);
  }

  logOff() {
    localStorage.removeItem('loginUser');
    localStorage.removeItem('myLogInToken');
    localStorage.removeItem('userRole');
    this.router.navigateByUrl('login');
  }
}