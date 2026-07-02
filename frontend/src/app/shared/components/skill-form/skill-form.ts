import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Skill, SkillCategory } from '../../../core/services/skills';
import { Modal } from '../modal/modal';
import { ImageUploader } from '../image-uploader/image-uploader';

@Component({
  selector: 'app-skill-form',
  standalone: true,
  imports: [CommonModule, FormsModule, Modal, ImageUploader],
  templateUrl: './skill-form.html',
  styleUrl: './skill-form.scss',
})
export class SkillForm {
  @Input() category: SkillCategory = SkillCategory.Soft;
  
  @Output() save = new EventEmitter<Skill>();
  @Output() cancel = new EventEmitter<void>();

  skillData: Partial<Skill> = {
    name: '',
    description: '',
    proficiencyLevel: 50,
    iconUrl: '',
    displayOrder: 99
  };

  ngOnInit() {
    this.skillData.category = this.category;
  }

  onSave(): void {
    if (!this.skillData.name?.trim()) {
      alert('O nome da habilidade é obrigatório.');
      return;
    }
    this.save.emit(this.skillData as Skill);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
