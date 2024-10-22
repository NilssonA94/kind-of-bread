import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [MatButtonModule, RouterModule, MatIconModule, MatFormFieldModule, MatInputModule, FormsModule, MatMenuModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss'
})
export class NavComponent {
  private router = inject(Router);

  @Input() theme!: string;
  @Output() themeChange = new EventEmitter<void>();

  searchValue: string = '';

  types: string[] = ["LiquidBread", "SimpleSandwich", "DoubleSandwich", "MultiSandwich", "Boatlike", "Extruded"];

  changeTheme(): void {
    this.themeChange.emit();
  }

  search(): void {
    if (this.searchValue === '') { return }
    try {
      this.router.navigate(['/get'], { queryParams: { s: this.searchValue.trim().split(/\s+/).map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()).join('') } });
    } catch (error) {
      console.log(error);
    }
  }

  get(type: string): void {
    try {
      this.router.navigate(['/get'], { queryParams: { s: type } });
    } catch (error) {
      console.log(error);
    }
  }
}
