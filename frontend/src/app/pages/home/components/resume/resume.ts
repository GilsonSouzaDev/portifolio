import { Component, Input, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Skill } from '../../../../core/services/skills';

@Component({
  selector: 'app-resume',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './resume.html',
  styleUrl: './resume.scss',
})
export class Resume {
  @Input({ required: true }) softSkills: Skill[] = [];
  @Input({ required: true }) hardSkills: Skill[] = [];
  @Input({ required: true }) badges: Skill[] = [];

  activeSlide = signal(0);

  activeHeading = computed(() => {
    switch (this.activeSlide()) {
      case 0: return 'Minhas Soft Skills';
      case 1: return 'Minhas Hard Skills';
      case 2: return 'Meus Badges';
      default: return '';
    }
  });

  currentSkills = computed(() => {
    return this.activeSlide() === 0 ? this.softSkills : this.hardSkills;
  });

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
