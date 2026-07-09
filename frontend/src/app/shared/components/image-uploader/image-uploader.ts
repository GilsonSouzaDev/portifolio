import { environment } from '../../../../environments/environment';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { EditMode } from '../../../core/services/edit-mode';
import { ImageUrlPipe } from '../../pipes/image-url.pipe';

@Component({
  selector: 'app-image-uploader',
  standalone: true,
  imports: [CommonModule, ImageUrlPipe],
  templateUrl: './image-uploader.html',
  styleUrl: './image-uploader.scss',
})
export class ImageUploader {
  @Input() imageUrl = '';
  @Input() shape: 'diamond' | 'rectangle' | 'circle' = 'diamond';
  @Output() imageUrlChange = new EventEmitter<string>();

  uploading = false;
  error = '';

  constructor(
    private http: HttpClient,
    private editMode: EditMode,
  ) {}

  get isEditMode(): boolean {
    return this.editMode.isEditMode();
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.uploading = true;
    this.error = '';

    const formData = new FormData();
    formData.append('file', file);

    this.http.post<{ url: string }>(environment.apiUrl + '/images/upload', formData).subscribe({
      next: (response) => {
        this.uploading = false;
        this.imageUrl = response.url;
        this.imageUrlChange.emit(response.url);
      },
      error: () => {
        this.uploading = false;
        this.error = 'Falha ao enviar a imagem. Tente novamente.';
      },
    });
  }
}

