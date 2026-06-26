import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Profile, ProfileService } from '../../core/services/profile';
import { Skill, SkillsService } from '../../core/services/skills';
import { Project, ProjectsService } from '../../core/services/projects';
import { SocialLink, SocialLinksService } from '../../core/services/social-links';
import { InlineEditor } from '../../shared/components/inline-editor/inline-editor';
import { ImageUploader } from '../../shared/components/image-uploader/image-uploader';
import { EditMode } from '../../core/services/edit-mode';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, InlineEditor, ImageUploader],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  profile = signal<Profile | null>(null);
  skills = signal<Skill[]>([]);
  projects = signal<Project[]>([]);
  socialLinks = signal<SocialLink[]>([]);
  activePortfolioFilter = signal('All');
  activeSoftSkillSlide = signal(0);

  portfolioFilters = ['All', 'Dashboards', 'Web Apps', 'Integrations'];

  hardSkills = computed(() => this.skills().filter((s) => !this.isSoftSkill(s)));

  softSkills = computed(() => this.skills().filter((s) => this.isSoftSkill(s)));

  filteredProjects = computed(() => {
    const filter = this.activePortfolioFilter();
    const all = this.projects();
    if (filter === 'All') return all;
    return all.filter(
      (p) =>
        p.title.toLowerCase().includes(filter.toLowerCase()) ||
        p.technologies.toLowerCase().includes(filter.toLowerCase()),
    );
  });

  linkedInLink = computed(
    () => this.socialLinks().find((l) => l.platform.toLowerCase().includes('linkedin')) ?? null,
  );

  contactForm = { name: '', email: '', message: '' };

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

  setPortfolioFilter(filter: string): void {
    this.activePortfolioFilter.set(filter);
  }

  setSoftSkillSlide(index: number): void {
    this.activeSoftSkillSlide.set(index);
  }

  getTechTags(technologies: string): string[] {
    return technologies
      .split(',')
      .map((t) => t.trim())
      .filter(Boolean);
  }

  onContactSubmit(): void {
    // Integração com API será feita em fase posterior
  }

  private isSoftSkill(skill: Skill): boolean {
    return skill.category.toLowerCase().includes('soft');
  }
}
