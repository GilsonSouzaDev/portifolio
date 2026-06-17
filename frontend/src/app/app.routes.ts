import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home/home').then((m) => m.Home),
  },
  {
    path: 'auth',
    loadComponent: () => import('./pages/auth/admin-login/admin-login').then((m) => m.AdminLogin),
  },
  {
    path: 'auth/verify',
    loadComponent: () =>
      import('./pages/auth/admin-verify/admin-verify').then((m) => m.AdminVerify),
  },
  {
    path: 'admin',
    loadComponent: () =>
      import('./pages/admin/admin-dashboard/admin-dashboard').then((m) => m.AdminDashboard),
    canActivate: [authGuard],
  },
  {
    path: '**',
    redirectTo: '',
  },
];
