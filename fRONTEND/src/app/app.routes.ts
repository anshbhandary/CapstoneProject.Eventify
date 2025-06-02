import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CustomerDashboardComponent } from './components/customer-dashboard/customer-dashboard.component';
import { AboutComponent } from './components/about/about.component';
import { TeamComponent } from './components/team/team.component';
import { MyRequestComponent } from './components/my-requests/my-requests.component';
import { VendorsComponent } from './components/vendors-list/vendors-list.component';
import { ErrorComponent } from './components/error/error.component';
import { ProfileComponent } from './components/profile/profile.component';
import { VendorhomeComponent } from './components/vendor-home/vendor-home.component';
import { MyRequestVComponent } from './components/my-requests-v/my-requests-v.component';
import { PackagesComponent } from './components/packages/packages.component';
import { AuthGuard } from './auth.guard'; // Import guard

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '404', component: ErrorComponent },
  { path: 'about', component: AboutComponent },
  { path: 'team', component: TeamComponent },

  // Protected routes
  { path: 'dashboard/c', component: CustomerDashboardComponent, canActivate: [AuthGuard] },
  { path: 'dashboard/v', component: VendorhomeComponent, canActivate: [AuthGuard] },
  { path: 'my-requests', component: MyRequestComponent, canActivate: [AuthGuard] },
  { path: 'vendors', component: VendorsComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'my-requests-v', component: MyRequestVComponent, canActivate: [AuthGuard] },
  { path: 'packages', component: PackagesComponent, canActivate: [AuthGuard] },

  { path: '**', redirectTo: '/404' }
];
