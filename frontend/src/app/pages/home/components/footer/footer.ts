import { Component } from '@angular/core';
import { EditToggleButton } from '../../../../shared/components/edit-toggle-button/edit-toggle-button';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [EditToggleButton],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class Footer {}
