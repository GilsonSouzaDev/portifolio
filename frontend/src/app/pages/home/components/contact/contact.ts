import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Profile } from '../../../../core/services/profile';
import { SocialLink } from '../../../../core/services/social-links';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.html',
  styleUrl: './contact.scss',
})
export class Contact {
  @Input() profile: Profile | null = null;
  @Input() socialLinks: SocialLink[] = [];

  contactForm = { name: '', email: '', message: '' };

  onContactSubmit(): void {
    // Integração com API será feita em fase posterior
    console.log('Form submitted:', this.contactForm);
  }
}
