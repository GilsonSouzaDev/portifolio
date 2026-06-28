import { Component, Input, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Project } from '../../../../core/services/projects';

@Component({
  selector: 'app-portfolio',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './portfolio.html',
  styleUrl: './portfolio.scss',
})
export class Portfolio {
  @Input({ required: true }) projects: Project[] = [];

  activePortfolioFilter = signal('All');
  portfolioFilters = ['All', 'Dashboards', 'Web Apps', 'Integrations'];

  filteredProjects = computed(() => {
    const filter = this.activePortfolioFilter();
    const all = this.projects;
    if (filter === 'All') return all;
    return all.filter(
      (p) =>
        p.title.toLowerCase().includes(filter.toLowerCase()) ||
        p.technologies.toLowerCase().includes(filter.toLowerCase()),
    );
  });

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
