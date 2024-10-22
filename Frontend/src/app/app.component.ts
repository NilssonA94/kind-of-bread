import { Component, HostBinding, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './services/theme.service';
import { NavComponent } from "./components/nav/nav.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Kind of Bread';
  @HostBinding('class') theme!: string;

  private themeService = inject(ThemeService)

  ngOnInit(): void {
    this.theme = this.themeService.loadTheme();
  }

  changeTheme(): void {
    try {
      this.theme = this.theme === 'dark-theme' ? 'light-theme' : 'dark-theme';
      this.themeService.saveTheme(this.theme);
    } catch (error) {
      console.error('Failed to handle theme:', error);
    }
  }
}
