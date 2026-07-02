import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Project } from '../../../core/services/projects';
import { Modal } from '../modal/modal';
import { ImageUploader } from '../image-uploader/image-uploader';

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [CommonModule, FormsModule, Modal, ImageUploader],
  templateUrl: './project-form.html',
  styleUrl: './project-form.scss',
})
export class ProjectForm {
  @Output() save = new EventEmitter<Project>();
  @Output() cancel = new EventEmitter<void>();

  projectData: Partial<Project> = {
    title: '',
    description: '',
    thumbnailUrl: '',
    technologies: '',
    repositoryUrl: '',
    projectUrl: ''
  };

  onSave(): void {
    if (!this.projectData.title?.trim()) {
      alert('O título do projeto é obrigatório.');
      return;
    }
    this.save.emit(this.projectData as Project);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
