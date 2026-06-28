import { Component, Input, signal } from '@angular/core';
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

  activeSoftSkillSlide = signal(0);

  setSoftSkillSlide(index: number): void {
    this.activeSoftSkillSlide.set(index);
  }
}
