import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Modal } from '../modal/modal';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, Modal],
  templateUrl: './confirm-dialog.html',
  styleUrl: './confirm-dialog.scss',
})
export class ConfirmDialog {
  @Input() title = 'Confirmação';
  @Input() message = 'Tem certeza que deseja realizar esta ação?';
  @Input() confirmText = 'Confirmar';
  @Input() cancelText = 'Cancelar';
  
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm(): void {
    this.confirm.emit();
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
