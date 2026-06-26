import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './shared/components/navbar/navbar';
import { EditToggleButton } from './shared/components/edit-toggle-button/edit-toggle-button';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, EditToggleButton],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');
}