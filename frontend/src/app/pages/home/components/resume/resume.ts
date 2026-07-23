import { Component, Input, Output, EventEmitter, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Skill, SkillCategory } from '../../../../core/services/skills';
import { InlineEditor } from '../../../../shared/components/inline-editor/inline-editor';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog/confirm-dialog';
import { SkillForm } from '../../../../shared/components/skill-form/skill-form';
import { ImageUrlPipe } from '../../../../shared/pipes/image-url.pipe';

@Component({
  selector: 'app-resume',
  standalone: true,
  imports: [CommonModule, InlineEditor, ConfirmDialog, SkillForm, ImageUrlPipe],
  templateUrl: './resume.html',
  styleUrl: './resume.scss',
})
export class Resume {
  @Input({ required: true }) softSkills: Skill[] = [];
  @Input({ required: true }) hardSkills: Skill[] = [];
  @Input({ required: true }) badges: Skill[] = [];
  @Input() isEditMode: boolean = false;

  @Output() skillUpdate = new EventEmitter<Skill>();
  @Output() skillDelete = new EventEmitter<number>();
  @Output() skillCreate = new EventEmitter<Skill>();

  activeSlide = signal(0);
  
  skillToDelete: number | null = null;
  isAddingSkill = false;
  selectedCategory: SkillCategory = SkillCategory.Soft;

  constructor() {}

  onSkillUpdate(skill: Skill, field: keyof Skill, value: string | number): void {
    const updated = { ...skill, [field]: value };
    this.skillUpdate.emit(updated);
  }

  requestSkillDelete(id: number): void {
    this.skillToDelete = id;
  }

  confirmSkillDelete(): void {
    if (this.skillToDelete !== null) {
      this.skillDelete.emit(this.skillToDelete);
      this.skillToDelete = null;
    }
  }

  cancelSkillDelete(): void {
    this.skillToDelete = null;
  }

  openSkillForm(): void {
    this.selectedCategory = this.activeSlide() === 0 ? SkillCategory.Soft : (this.activeSlide() === 1 ? SkillCategory.Hard : SkillCategory.Badge);
    this.isAddingSkill = true;
  }

  saveNewSkill(skill: Skill): void {
    this.skillCreate.emit(skill);
    this.isAddingSkill = false;
  }

  cancelSkillForm(): void {
    this.isAddingSkill = false;
  }

  activeHeading = computed(() => {
    switch (this.activeSlide()) {
      case 0: return 'Minhas Soft Skills';
      case 1: return 'Minhas Hard Skills';
      case 2: return 'Meus Badges';
      default: return '';
    }
  });

  get currentSkills(): Skill[] {
    return this.activeSlide() === 0 ? this.softSkills : this.hardSkills;
  }

  setSlide(index: number): void {
    this.activeSlide.set(index);
  }

  prevSlide(): void {
    const current = this.activeSlide();
    this.activeSlide.set(current > 0 ? current - 1 : 2);
  }

  nextSlide(): void {
    const current = this.activeSlide();
    this.activeSlide.set(current < 2 ? current + 1 : 0);
  }
}
