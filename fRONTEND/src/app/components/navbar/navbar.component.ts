import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
  ],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  isLogged = false;
  userRole: string | null = null;
  @ViewChild('navbarCollapse') navbarCollapse!: ElementRef;
  isNavbarCollapsed = true;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.isLoggedIn$.subscribe((status) => {
      this.isLogged = status;
      this.userRole = this.authService.getUserRole();
    });
  }

  toggleNavbar() {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
    if (this.navbarCollapse) {
      if (this.isNavbarCollapsed) {
        this.navbarCollapse.nativeElement.classList.remove('show');
      } else {
        this.navbarCollapse.nativeElement.classList.add('show');
      }
    }
  }

  closeNavbar() {
    this.isNavbarCollapsed = true;
    if (this.navbarCollapse) {
      this.navbarCollapse.nativeElement.classList.remove('show');
    }
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  goToVendorsOrPackages() {
    if (this.userRole === 'Customer') {
      this.router.navigate(['/vendors']);
    } else if (this.userRole === 'Vendor') {
      this.router.navigate(['/packages']);
    }
    this.closeNavbar();
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }

  goToMyRequests() {
    if (this.userRole === 'Vendor') {
      this.router.navigate(['/my-requests-v']);
    } else if (this.userRole === 'Customer') {
      this.router.navigate(['/my-requests']);
    }
    this.closeNavbar();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}