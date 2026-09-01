import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RefreshService } from '../../../core/services/refresh';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  menuOpen = signal(false);

  links = [
    { label: 'Home', href: '#hero' },
    { label: 'Sobre', href: '#sobre' },
    { label: 'Resumo', href: '#resumo' },
    { label: 'Portfólio', href: '#portfolio' },
    { label: 'Contato', href: '#contato' },
  ];

  constructor(private refreshService: RefreshService) {}

  toggleMenu(): void {
    this.menuOpen.update((value) => !value);
  }

  closeMenu(): void {
    this.menuOpen.set(false);
  }

  onNavClick(): void {
    this.refreshService.triggerRefresh();
    this.closeMenu();
  }
}
