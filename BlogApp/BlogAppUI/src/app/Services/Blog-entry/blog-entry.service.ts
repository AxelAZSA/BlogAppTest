import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BlogEntry } from '../../interfaces/blog-entry.model';
import { environment } from '../../environment/enviroment';

@Injectable({
  providedIn: 'root',
})
export class BlogEntryService {
  private apiUrl = environment.BaseApiUrl+'/blogentries';  // URL de tu API

  constructor(private http: HttpClient) { }

  // Obtener todas las entradas de blog
  getAll(): Observable<BlogEntry[]> {
    return this.http.get<BlogEntry[]>(`${this.apiUrl}/all`);
  }

  // Obtener una entrada de blog por ID
  getById(id: number): Observable<BlogEntry> {
    return this.http.get<BlogEntry>(`${this.apiUrl}/${id}`);
  }

  // Crear una nueva entrada de blog
  create(entry: BlogEntry): Observable<BlogEntry> {
    return this.http.post<BlogEntry>(`${this.apiUrl}/create`, entry);
  }

  // Actualizar una entrada de blog existente
  update(id: number, entry: BlogEntry): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/edit/${id}`, entry);
  }

  // Eliminar una entrada de blog por ID
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  
  uploadImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', file);
    return this.http.post<any>(`${this.apiUrl}/upload`, formData);
  }
}
