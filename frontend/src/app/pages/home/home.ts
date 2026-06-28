import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Profile, ProfileService } from '../../core/services/profile';
import { Skill, SkillsService } from '../../core/services/skills';
import { Project, ProjectsService } from '../../core/services/projects';
import { SocialLink, SocialLinksService } from '../../core/services/social-links';
import { EditMode } from '../../core/services/edit-mode';

import { Hero } from './components/hero/hero';
import { About } from './components/about/about';
import { Resume } from './components/resume/resume';
import { Portfolio } from './components/portfolio/portfolio';
import { Contact } from './components/contact/contact';
import { Footer } from './components/footer/footer';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, Hero, About, Resume, Portfolio, Contact, Footer],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  profile = signal<Profile | null>(null);
  skills = signal<Skill[]>([]);
  projects = signal<Project[]>([]);
  socialLinks = signal<SocialLink[]>([]);

  hardSkills = computed(() => this.skills().filter((s) => !this.isSoftSkill(s)));
  softSkills = computed(() => this.skills().filter((s) => this.isSoftSkill(s)));

  linkedInLink = computed(
    () => this.socialLinks().find((l) => l.platform.toLowerCase().includes('linkedin')) ?? null,
  );

  constructor(
    private profileService: ProfileService,
    private skillsService: SkillsService,
    private projectsService: ProjectsService,
    private socialLinksService: SocialLinksService,
    private editMode: EditMode,
  ) {}

  get isEditMode(): boolean {
    return this.editMode.isEditMode();
  }

  ngOnInit(): void {
    this.profileService.get().subscribe((data) => this.profile.set(data));
    this.skillsService.getAll().subscribe((data) => this.skills.set(data));
    this.projectsService.getAll().subscribe((data) => this.projects.set(data));
    this.socialLinksService.getAll().subscribe((data) => this.socialLinks.set(data));
  }

  onProfileFieldChange(field: keyof Profile, value: string): void {
    const current = this.profile();
    if (!current) return;
    const updated = { ...current, [field]: value };
    this.profile.set(updated);
    this.profileService.update(updated).subscribe();
  }

  onAvatarChange(url: string): void {
    const current = this.profile();
    if (!current) return;
    const updated = { ...current, avatarUrl: url };
    this.profile.set(updated);
    this.profileService.update(updated).subscribe();
  }

  private isSoftSkill(skill: Skill): boolean {
    return skill.category.toLowerCase().includes('soft');
  }
}
