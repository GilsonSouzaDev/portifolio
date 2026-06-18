import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditMode } from '../../../core/services/edit-mode';
import { Auth } from '../../../core/services/auth';

@Component({
  selector: 'app-edit-toggle-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-toggle-button.html',
  styleUrl: './edit-toggle-button.scss',
})
export class EditToggleButton {
  constructor(
    private editMode: EditMode,
    private auth: Auth,
  ) {}

  get isAuthenticated(): boolean {
    return this.auth.isAuthenticated();
  }

  get isEditMode(): boolean {
    return this.editMode.isEditMode();
  }

  toggle(): void {
    this.editMode.toggle();
  }
}
