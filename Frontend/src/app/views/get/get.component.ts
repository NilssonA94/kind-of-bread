import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Bread } from '../../models/types';
import { BreadService } from '../../services/bread.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-get',
  standalone: true,
  imports: [MatCardModule, MatListModule, MatButtonModule, MatIconModule],
  templateUrl: './get.component.html',
  styleUrl: './get.component.scss'
})
export class GetComponent {
  private breadService = inject(BreadService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  breads?: any[];
  types: string[] = ["LiquidBread", "SimpleSandwich", "DoubleSandwich", "MultiSandwich", "Boatlike", "Extruded"];

  ngOnInit(): void {
    this.getService();
  }

  getService(): void {
    this.route.queryParams.subscribe(params => {
      const type = params['s'];

      if (type === 'All') {
        this.breadService.getAllBread().subscribe(fetchedBread => this.breads = fetchedBread);
      } else if (this.types.includes(type)) {
        this.breadService.getBreadByType(type).subscribe(fetchedBread => this.breads = fetchedBread);
      } else if (Number.isInteger(Number(type))) {
        this.breadService.getBreadById(type).subscribe(fetchedBread => this.breads = fetchedBread);
      } else {
        this.breadService.getBreadByName(type).subscribe(fetchedBread => this.breads = fetchedBread);
      }
    });
  }

  editBread(bread: Bread): void {
    this.router.navigate(['/update'], { queryParams: { id: bread.id } });
  }

  deleteBread(bread: Bread): void {
    this.breadService.deleteBread(bread.id!).subscribe({
      next: () => this.breads?.splice(this.breads.findIndex(b => b.id === bread.id), 1),
      error: (e) => console.log(e),
      complete: () => console.log('Success!')
    });
  }

  getKeys(bread: Bread): string[] {
    return Object.keys(bread);
  }

  isArray(value: any): boolean {
    return Array.isArray(value);
  }

  isBoolean(value: any): boolean {
    return typeof value === 'boolean';
  }

  capitalizeFirstLetter(s: string): string {
    return s.charAt(0).toUpperCase() + s.slice(1);
  }

  splitCapitalizedWords(word: string): string {
    return word.replace(/([a-z])([A-Z])/g, '$1 $2').replace(/([A-Z])([A-Z][a-z])/g, '$1 $2');
  }
}
