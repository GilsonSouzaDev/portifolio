import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService, Profile } from '../../../core/services/profile';
import { SkillsService, Skill } from '../../../core/services/skills';
import { ProjectsService, Project } from '../../../core/services/projects';
import { SocialLinksService, SocialLink } from '../../../core/services/social-links';
import { InlineEditor } from '../../../shared/components/inline-editor/inline-editor';
import { ImageUploader } from '../../../shared/components/image-uploader/image-uploader';
import { EditMode } from '../../../core/services/edit-mode';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, InlineEditor, ImageUploader],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  profile = signal<Profile | null>(null);
  skills = signal<Skill[]>([]);
  projects = signal<Project[]>([]);
  socialLinks = signal<SocialLink[]>([]);

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
}
