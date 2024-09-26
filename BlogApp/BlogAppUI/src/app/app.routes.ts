import { Routes } from '@angular/router';
import { UserGuard } from './user.guard';
import { BlogEditComponent } from './blog-edit/blog-edit.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [  
    { path: 'login', component: LoginComponent},
    { path: 'blogEdit/:id', component: BlogEditComponent, canActivate:[UserGuard]},
    { path: 'user', component: UserComponent, canActivate:[UserGuard]},
    { path: '', component: HomeComponent, canActivate:[UserGuard]}
  ];
  
