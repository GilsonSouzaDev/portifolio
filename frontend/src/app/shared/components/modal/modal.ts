import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.html',
  styleUrl: './modal.scss',
})
export class Modal {
  @Input({ required: true }) title = '';
  @Output() close = new EventEmitter<void>();

  onClose(event?: Event): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }
    this.close.emit();
  }
}
