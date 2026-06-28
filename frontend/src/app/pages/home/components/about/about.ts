import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Profile } from '../../../../core/services/profile';
import { InlineEditor } from '../../../../shared/components/inline-editor/inline-editor';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule, InlineEditor],
  templateUrl: './about.html',
  styleUrl: './about.scss',
})
export class About {
  @Input({ required: true }) profile!: Profile;
  @Output() profileFieldChange = new EventEmitter<{ field: keyof Profile; value: string }>();

  onFieldChange(field: keyof Profile, value: string): void {
    this.profileFieldChange.emit({ field, value });
  }
}
