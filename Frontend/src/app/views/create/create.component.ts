import { Component, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Bread } from '../../models/types';
import { BreadService } from '../../services/bread.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-create',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, FormsModule, MatSelectModule, MatButtonModule, MatRadioModule, ReactiveFormsModule, MatListModule, MatIconModule],
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss'
})
export class CreateComponent {
  private breadService = inject(BreadService);
  private router = inject(Router);

  types: string[] = ["LiquidBread", "SimpleSandwich", "DoubleSandwich", "MultiSandwich", "Boatlike", "Extruded"];
  grains: string[] = ["Wheat", "Rye", "Barley"];
  topping: string = '';
  filling: string = '';

  creationGroup = inject(FormBuilder).group({
    $type: '',
    grain: '',
    name: '',
    fermented: null,
    viscosity: null,
    texture: null,
    dressed: null,
    shape: null,
    toppings: null as string[] | null,
    fillings: null as string[] | null
  });

  addTopping(): void {
    if (this.topping.trim()) {
      if (this.creationGroup.value.toppings === null) {
        this.creationGroup.patchValue({ toppings: [this.topping] });
      } else {
        this.creationGroup.patchValue({ toppings: [...this.creationGroup.value.toppings!, this.topping] });
      }
    }
    this.topping = '';
  }

  addFilling(): void {
    if (this.filling.trim()) {
      if (this.creationGroup.value.fillings === null) {
        this.creationGroup.patchValue({ fillings: [this.filling] });
      } else if (!this.creationGroup.value.fillings!.includes(this.filling.trim())) {
        this.creationGroup.patchValue({ fillings: [...this.creationGroup.value.fillings!, this.filling] });
      }
    }
    this.filling = '';
  }

  deleteTopping(topping: string): void {
    this.creationGroup.value.toppings?.splice(this.creationGroup.value.toppings.indexOf(topping), 1);
  }

  deleteFilling(filling: string): void {
    this.creationGroup.value.fillings?.splice(this.creationGroup.value.fillings.indexOf(filling), 1);
  }

  createBread(): void {
    if (this.creationGroup.value.$type !== "SimpleSandwich" && this.creationGroup.value.$type !== "MultiSandwich") {
      this.creationGroup.value.toppings = null;
    }

    if (this.creationGroup.value.$type !== "DoubleSandwich" && this.creationGroup.value.$type !== "MultiSandwich" && this.creationGroup.value.$type !== "Boatlike") {
      this.creationGroup.value.fillings = null;
    }

    this.breadService.createBread(JSON.parse(JSON.stringify(Object.fromEntries(Object.entries(this.creationGroup.value).filter(([_, value]) => value !== null)))) as Bread).subscribe({
      next: (n) => this.router.navigate(['/get'], { queryParams: { s: n.name.replace(/\s/g, '') } }),
      error: (e) => console.log(e),
      complete: () => console.log('complete')
    });
  }
}
