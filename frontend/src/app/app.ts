import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { EditToggleButton } from "./shared/components/edit-toggle-button/edit-toggle-button";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, EditToggleButton],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');
}
