import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

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
    private http: HttpClient,
    private router: Router,
  ) {}

  requestCode() {
    this.loading = true;
    this.error = '';

    this.http.post('http://localhost:5217/api/auth/request-link', {}).subscribe({
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
