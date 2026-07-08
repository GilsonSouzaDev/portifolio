import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../core/services/auth';
import { EditMode } from '../../../core/services/edit-mode';

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
    private auth: Auth,
    private router: Router,
    private editMode: EditMode
  ) {}

  verify() {
    if (!this.code || this.code.length !== 6) {
      this.error = 'Digite o código de 6 dígitos.';
      return;
    }

    this.loading = true;
    this.error = '';

    this.auth.verifyCode(this.code).subscribe({
      next: () => {
        this.loading = false;
        this.editMode.enable();
        this.router.navigate(['/']);
      },
      error: () => {
        this.loading = false;
        this.error = 'Código inválido ou expirado. Tente novamente.';
      },
    });
  }
}
