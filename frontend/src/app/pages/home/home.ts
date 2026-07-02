import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Profile, ProfileService } from '../../core/services/profile';
import { Skill, SkillCategory, SkillsService } from '../../core/services/skills';
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

  softSkills = computed(() =>
    this.skills()
      .filter((s) => s.category === SkillCategory.Soft)
      .sort((a, b) => b.proficiencyLevel - a.proficiencyLevel),
  );

  hardSkills = computed(() =>
    this.skills()
      .filter((s) => s.category === SkillCategory.Hard)
      .sort((a, b) => b.proficiencyLevel - a.proficiencyLevel),
  );

  badges = computed(() => this.skills().filter((s) => s.category === SkillCategory.Badge));

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

  onSocialLinkUpdate(link: SocialLink): void {
    this.socialLinks.update((links) => links.map((l) => (l.id === link.id ? link : l)));
    this.socialLinksService.update(link.id, link).subscribe();
  }

  onSkillUpdate(skill: Skill): void {
    this.skills.update((skills) => skills.map((s) => (s.id === skill.id ? skill : s)));
    this.skillsService.update(skill.id, skill).subscribe();
  }

  onSkillDelete(id: number): void {
    this.skills.update((skills) => skills.filter((s) => s.id !== id));
    this.skillsService.delete(id).subscribe();
  }

  onSkillCreate(newSkill: Skill): void {
    this.skillsService.create(newSkill).subscribe((created) => {
      this.skills.update((skills) => [...skills, created]);
    });
  }

  onProjectUpdate(project: Project): void {
    this.projects.update((projects) => projects.map((p) => (p.id === project.id ? project : p)));
    this.projectsService.update(project.id, project).subscribe();
  }

  onProjectDelete(id: number): void {
    this.projects.update((projects) => projects.filter((p) => p.id !== id));
    this.projectsService.delete(id).subscribe();
  }

  onProjectCreate(newProject: Project): void {
    this.projectsService.create(newProject).subscribe((created) => {
      this.projects.update((projects) => [...projects, created]);
    });
  }
}
