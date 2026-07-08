import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Profile } from '../../../../core/services/profile';
import { SocialLink } from '../../../../core/services/social-links';
import { InlineEditor } from '../../../../shared/components/inline-editor/inline-editor';
import { ImageUploader } from '../../../../shared/components/image-uploader/image-uploader';

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [CommonModule, InlineEditor, ImageUploader],
  templateUrl: './hero.html',
  styleUrl: './hero.scss',
})
export class Hero {
  @Input({ required: true }) profile!: Profile;
  @Input() linkedInLink: SocialLink | null = null;
  @Input() isEditMode: boolean = false;
  @Output() profileFieldChange = new EventEmitter<{ field: keyof Profile; value: string }>();
  @Output() avatarChange = new EventEmitter<string>();
  @Output() socialLinkChange = new EventEmitter<SocialLink>();

  constructor() {}

  onSocialClick(event: Event, link: SocialLink): void {
    if (this.isEditMode) {
      event.preventDefault();
      const newUrl = prompt('Qual a nova URL do seu LinkedIn?', link.url);
      if (newUrl !== null && newUrl !== link.url) {
        this.socialLinkChange.emit({ ...link, url: newUrl });
      }
    }
  }

  onFieldChange(field: keyof Profile, value: string): void {
    this.profileFieldChange.emit({ field, value });
  }

  onAvatarChange(url: string): void {
    this.avatarChange.emit(url);
  }
}
