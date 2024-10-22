import { Component, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
  selector: 'app-update',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, FormsModule, MatSelectModule, MatButtonModule, MatRadioModule, ReactiveFormsModule, MatListModule, MatIconModule],
  templateUrl: './update.component.html',
  styleUrl: './update.component.scss'
})
export class UpdateComponent {
  private breadService = inject(BreadService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  types: string[] = ["LiquidBread", "SimpleSandwich", "DoubleSandwich", "MultiSandwich", "Boatlike", "Extruded"];
  grains: string[] = ["Wheat", "Rye", "Barley"];
  topping: string = '';
  filling: string = '';

  updateGroup = inject(FormBuilder).group({
    $type: '',
    id: 0,
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

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.breadService.getBreadById(params['id']).subscribe(bread => {
        this.updateGroup.patchValue(bread[0] as any);
      })
    });
  }

  addTopping(): void {
    if (this.topping.trim()) {
      if (this.updateGroup.value.toppings === null) {
        this.updateGroup.patchValue({ toppings: [this.topping] });
      } else {
        this.updateGroup.patchValue({ toppings: [...this.updateGroup.value.toppings!, this.topping] });
      }
    }
    this.topping = '';
  }

  addFilling(): void {
    if (this.filling.trim()) {
      if (this.updateGroup.value.fillings === null) {
        this.updateGroup.patchValue({ fillings: [this.filling] });
      } else if (!this.updateGroup.value.fillings!.includes(this.filling.trim())) {
        this.updateGroup.patchValue({ fillings: [...this.updateGroup.value.fillings!, this.filling] });
      }
    }
    this.filling = '';
  }

  deleteTopping(topping: string): void {
    this.updateGroup.value.toppings?.splice(this.updateGroup.value.toppings.indexOf(topping), 1);
  }

  deleteFilling(filling: string): void {
    this.updateGroup.value.fillings?.splice(this.updateGroup.value.fillings.indexOf(filling), 1);
  }

  updateBread(): void {
    if (this.updateGroup.value.$type !== "SimpleSandwich" && this.updateGroup.value.$type !== "MultiSandwich") {
      this.updateGroup.value.toppings = null;
    }

    if (this.updateGroup.value.$type !== "DoubleSandwich" && this.updateGroup.value.$type !== "MultiSandwich" && this.updateGroup.value.$type !== "Boatlike") {
      this.updateGroup.value.fillings = null;
    }

    this.breadService.updateBread(JSON.parse(JSON.stringify(Object.fromEntries(Object.entries(this.updateGroup.value).filter(([_, value]) => value !== null)))) as Bread).subscribe({
      next: () => this.router.navigate(['/get'], { queryParams: { s: this.updateGroup.value.id } }),
      error: (e) => console.log(e),
      complete: () => console.log('complete')
    });
  }
}
