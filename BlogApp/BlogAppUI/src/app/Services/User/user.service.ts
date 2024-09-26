import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthResponse, LoginDto, RefreshRequest, RegisterDTO, User } from '../../interfaces/user.model';
import { environment } from '../../environment/enviroment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.BaseApiUrl+'/users';  // URL de tu API

  constructor(private http: HttpClient) { }

  // Obtener todos los usuarios (solo Admin)
  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/all`);
  }

  // Obtener el usuario actual
  getCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/MyUser`);
  }

  // Registrar un nuevo usuario
  register(user: RegisterDTO): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, user);
  }

  // Actualizar un usuario por ID
  updateUser(id: number, user: User): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/edit/${id}`, user);
  }

  // Iniciar sesión
  login(loginData: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, loginData);
  }

  // Cerrar sesión
  logout(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/logout`);
  }

  // Refrescar el token
  refresh(refreshData: RefreshRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh`, refreshData);
  }
}
