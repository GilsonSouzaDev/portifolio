import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-verify',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-verify.html',
  styleUrl: './admin-verify.scss',
})
export class AdminVerify {
  code = '';
  loading = false;
  error = '';

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  verify() {
    if (!this.code || this.code.length !== 6) {
      this.error = 'Digite o código de 6 dígitos.';
      return;
    }

    this.loading = true;
    this.error = '';

    this.http
      .get<{ sessionToken: string }>(`http://localhost:5217/api/auth/verify?token=${this.code}`)
      .subscribe({
        next: (response) => {
          localStorage.setItem('sessionToken', response.sessionToken);
          this.loading = false;
          this.router.navigate(['/admin']);
        },
        error: () => {
          this.loading = false;
          this.error = 'Código inválido ou expirado. Tente novamente.';
        },
      });
  }
}
