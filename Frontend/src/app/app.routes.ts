import { Routes } from '@angular/router';
import { CreateComponent } from './views/create/create.component';
import { GetComponent } from './views/get/get.component';
import { HomeComponent } from './views/home/home.component';
import { UpdateComponent } from './views/update/update.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'get', component: GetComponent },
    { path: 'create', component: CreateComponent },
    { path: 'update', component: UpdateComponent }
];
