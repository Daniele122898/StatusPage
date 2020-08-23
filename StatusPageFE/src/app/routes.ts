import {Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {AdminDashboardComponent} from './admin-dashboard/pages/admin-dashboard/admin-dashboard.component';
import {LoginComponent} from './admin-dashboard/pages/login/login.component';

export const appRoutes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'dashboard',
    component: AdminDashboardComponent
  },
  {
    path: 'login',
    component: LoginComponent
  }
];
