import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private theme = signal<string>(this.loadTheme());

  private getPreferredTheme(): string {
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark-theme' : 'light-theme';
  }

  public saveTheme(theme: string): void {
    try {
      localStorage.setItem('theme', theme);
      this.theme.set(theme);
    } catch (error) {
      console.error('Error saving theme to local storage', error);
    }
  }

  public loadTheme(): string {
    return localStorage.getItem('theme') ?? this.getPreferredTheme();
  }

  public getTheme() {
    return this.theme;
  }
}
