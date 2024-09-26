import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../../interfaces/category.model';
import { environment } from '../../environment/enviroment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = environment.BaseApiUrl+'/categories';  // URL de tu API

  constructor(private http: HttpClient) { }

  // Obtener todas las categorías
  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/all`);
  }

  // Obtener una categoría por ID
  getById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  // Crear una nueva categoría
  create(category: Category): Observable<Category> {
    return this.http.post<Category>(`${this.apiUrl}/create`, category);
  }

  // Actualizar una categoría existente
  update(id: number, category: Category): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/edit/${id}`, category);
  }

  // Eliminar una categoría por ID
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
