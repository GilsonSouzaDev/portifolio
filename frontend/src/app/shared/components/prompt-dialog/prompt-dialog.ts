import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Modal } from '../modal/modal';

@Component({
  selector: 'app-prompt-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, Modal],
  templateUrl: './prompt-dialog.html',
  styleUrl: './prompt-dialog.scss',
})
export class PromptDialog implements OnInit {
  @Input() title = 'Editar';
  @Input() message = 'Insira o novo valor:';
  @Input() initialValue = '';
  @Input() confirmText = 'Salvar';
  @Input() cancelText = 'Cancelar';
  
  @Output() confirm = new EventEmitter<string>();
  @Output() cancel = new EventEmitter<void>();

  value = '';

  ngOnInit(): void {
    this.value = this.initialValue;
  }

  onConfirm(): void {
    this.confirm.emit(this.value);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
