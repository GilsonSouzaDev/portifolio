import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EditMode } from '../../../core/services/edit-mode';

@Component({
  selector: 'app-inline-editor',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inline-editor.html',
  styleUrl: './inline-editor.scss',
})
export class InlineEditor {
  @Input() value = '';
  @Input() multiline = false;
  @Output() valueChange = new EventEmitter<string>();

  editing = false;
  draft = '';

  constructor(private editMode: EditMode) {}

  get isEditMode(): boolean {
    return this.editMode.isEditMode();
  }

  startEditing(): void {
    if (!this.isEditMode) return;
    this.draft = this.value;
    this.editing = true;
  }

  save(): void {
    this.editing = false;
    if (this.draft !== this.value) {
      this.value = this.draft;
      this.valueChange.emit(this.draft);
    }
  }

  cancel(): void {
    this.editing = false;
    this.draft = this.value;
  }
}
