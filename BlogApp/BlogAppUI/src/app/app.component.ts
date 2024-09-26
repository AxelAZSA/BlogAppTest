import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterModule, RouterOutlet } from '@angular/router';
import { LoginComponent } from "./login/login.component";
import { HomeComponent } from "./home/home.component";
import { UserService } from './Services/User/user.service';
import { CookieService } from 'ngx-cookie-service';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule,RouterOutlet,RouterModule, LoginComponent, HomeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'BlogAppUI';
  islogged: boolean =false;
  constructor(private userService: UserService,private router:Router, private cookieService: CookieService)
  {
    this.isLoggedIn();
  }

  isLoggedIn() {
    this.islogged = this.cookieService.check('token'); // Cambia 'token' por el nombre de tu cookie
  }

  logout() {
    this.userService.logout().subscribe(
      () => {
        this.cookieService.delete('token');
        this.router.navigate(['login']);
        this.islogged=false;
      },
      (error) => {
        console.error('Error during logout:', error);
        // Manejo del error si es necesario
      }
    );
  }

  ngOnInit() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd)) // Filtra solo NavigationEnd
      .subscribe(() => {
        this.onRouteChange(); // Llama a tu método cuando cambie la ruta
      });
  }

  onRouteChange() {
   this.isLoggedIn()
    // Aquí puedes implementar la lógica que quieras ejecutar
  }
}