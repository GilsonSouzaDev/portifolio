import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { EditToggleButton } from "./shared/components/edit-toggle-button/edit-toggle-button";
import { Navbar } from "./shared/components/navbar/navbar";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, EditToggleButton, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');
}
