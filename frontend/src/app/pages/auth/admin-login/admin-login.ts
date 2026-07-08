import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../core/services/auth';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-login.html',
  styleUrl: './admin-login.scss',
})
export class AdminLogin {
  loading = false;
  sent = false;
  error = '';

  constructor(
    private auth: Auth,
    private router: Router,
  ) {}

  requestCode() {
    this.loading = true;
    this.error = '';

    this.auth.requestCode().subscribe({
      next: () => {
        this.loading = false;
        this.sent = true;
        this.router.navigate(['/auth/verify']);
      },
      error: () => {
        this.loading = false;
        this.error = 'Falha ao enviar o código. Tente novamente.';
      },
    });
  }
}
