import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditMode {
  private editModeSignal = signal<boolean>(false);

  isEditMode = this.editModeSignal.asReadonly();

  toggle(): void {
    this.editModeSignal.update((value) => !value);
  }

  enable(): void {
    this.editModeSignal.set(true);
  }

  disable(): void {
    this.editModeSignal.set(false);
  }
}
