import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Bread } from '../models/types';

@Injectable({
  providedIn: 'root'
})
export class BreadService {
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5177/Bread';

  getAllBread(): Observable<Bread[]> {
    return this.http.get<Bread[]>(`${this.apiUrl}/Get/All`);
  }

  getBreadById(id: number): Observable<Bread[]> {
    return this.http.get<Bread[]>(`${this.apiUrl}/Get/ById/${id}`);
  }

  getBreadByName(name: string): Observable<Bread[]> {
    return this.http.get<Bread[]>(`${this.apiUrl}/Get/ByName/${name}`);
  }

  getBreadByType(type: string): Observable<Bread[]> {
    return this.http.get<Bread[]>(`${this.apiUrl}/Get/All/${type}`);
  }

  updateBread(bread: Bread): Observable<string> {
    return this.http.put(`${this.apiUrl}/Update/${bread.id}`, bread, { responseType: 'text' });
  }

  deleteBread(id: number): Observable<string> {
    return this.http.delete(`${this.apiUrl}/Delete/${id}`, { responseType: 'text' });
  }

  createBread(bread: Bread): Observable<Bread> {
    return this.http.post<Bread>(`${this.apiUrl}/Create`, bread);
  }
}
