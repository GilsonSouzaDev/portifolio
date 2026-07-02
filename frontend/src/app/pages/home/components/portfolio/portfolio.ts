import { Component, Input, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Project } from '../../../../core/services/projects';
import { InlineEditor } from '../../../../shared/components/inline-editor/inline-editor';
import { ImageUploader } from '../../../../shared/components/image-uploader/image-uploader';
import { EditMode } from '../../../../core/services/edit-mode';
import { Output, EventEmitter } from '@angular/core';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog/confirm-dialog';
import { ProjectForm } from '../../../../shared/components/project-form/project-form';
import { PromptDialog } from '../../../../shared/components/prompt-dialog/prompt-dialog';

@Component({
  selector: 'app-portfolio',
  standalone: true,
  imports: [CommonModule, InlineEditor, ImageUploader, ConfirmDialog, ProjectForm, PromptDialog],
  templateUrl: './portfolio.html',
  styleUrl: './portfolio.scss',
})
export class Portfolio {
  @Input({ required: true }) projects: Project[] = [];

  @Output() projectUpdate = new EventEmitter<Project>();
  @Output() projectDelete = new EventEmitter<number>();
  @Output() projectCreate = new EventEmitter<Project>();

  activePortfolioFilter = signal('All');
  portfolioFilters = ['All', 'Java', 'Angular', '.NET'];

  projectToDelete: number | null = null;
  isAddingProject = false;
  
  promptState: {
    isOpen: boolean;
    title: string;
    message: string;
    initialValue: string;
    project: Project | null;
    linkType: 'repositoryUrl' | 'projectUrl' | null;
  } = {
    isOpen: false,
    title: '',
    message: '',
    initialValue: '',
    project: null,
    linkType: null
  };

  constructor(private editMode: EditMode) {}

  get isEditMode(): boolean {
    return this.editMode.isEditMode();
  }

  onProjectUpdate(project: Project, field: keyof Project, value: string): void {
    const updated = { ...project, [field]: value };
    this.projectUpdate.emit(updated);
  }

  requestProjectDelete(id: number): void {
    this.projectToDelete = id;
  }

  confirmProjectDelete(): void {
    if (this.projectToDelete !== null) {
      this.projectDelete.emit(this.projectToDelete);
      this.projectToDelete = null;
    }
  }

  cancelProjectDelete(): void {
    this.projectToDelete = null;
  }

  openProjectForm(): void {
    this.isAddingProject = true;
  }

  saveNewProject(project: Project): void {
    this.projectCreate.emit(project);
    this.isAddingProject = false;
  }

  cancelProjectForm(): void {
    this.isAddingProject = false;
  }

  onLinkClick(event: Event, project: Project, linkType: 'repositoryUrl' | 'projectUrl'): void {
    if (this.isEditMode) {
      event.preventDefault();
      this.promptState = {
        isOpen: true,
        title: linkType === 'repositoryUrl' ? 'Editar GitHub' : 'Editar Demo',
        message: `Qual a nova URL do ${linkType === 'repositoryUrl' ? 'GitHub' : 'Demo'}?`,
        initialValue: project[linkType] || '',
        project,
        linkType
      };
    }
  }

  confirmPrompt(newUrl: string): void {
    if (this.promptState.project && this.promptState.linkType) {
      if (newUrl !== this.promptState.project[this.promptState.linkType]) {
        this.onProjectUpdate(this.promptState.project, this.promptState.linkType, newUrl);
      }
    }
    this.cancelPrompt();
  }

  cancelPrompt(): void {
    this.promptState.isOpen = false;
    this.promptState.project = null;
    this.promptState.linkType = null;
  }

  get filteredProjects(): Project[] {
    const filter = this.activePortfolioFilter();
    const all = this.projects;
    if (filter === 'All') return all;
    return all.filter(
      (p) =>
        p.title.toLowerCase().includes(filter.toLowerCase()) ||
        p.technologies.toLowerCase().includes(filter.toLowerCase()),
    );
  }

  setPortfolioFilter(filter: string): void {
    this.activePortfolioFilter.set(filter);
  }

  getTechTags(technologies: string): string[] {
    return technologies
      .split(',')
      .map((t) => t.trim())
      .filter(Boolean);
  }
}
