import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../Services/User/user.service';
import { LoginDto, RegisterDTO } from '../interfaces/user.model';
import { HttpClientModule } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterModule]  ,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  isRegistering: boolean = false;
  loginForm: FormGroup;
  registerForm: FormGroup;
  passwordsDoNotMatch: boolean = false;
  // Método que verifica si las contraseñas coinciden

  switchForm() {
    this.isRegistering = !this.isRegistering;
  }

  constructor(private fb: FormBuilder, private userService: UserService,private cookieService : CookieService,private router:Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }
  checkPasswords(): void {
    this.passwordsDoNotMatch = this.registerForm.get('password')?.value == this.registerForm?.get('confirmPassword');
  }

  onLogin() {
    if (this.loginForm.valid) {
      const loginData: LoginDto = this.loginForm.value;
      this.userService.login(loginData).subscribe(
        response => {
          console.log('Login successful', response);
          this.cookieService.set('token',response.token), this.router.navigate([''])
        },
        error => {
          alert('Login failed'+ error);
        }
      );
    }
  }

  onRegister() {
    if (this.registerForm.valid) {
      console.log(this.registerForm.get('password')?.value )
      console.log(this.registerForm.get('confirmPassword')?.value )
    if (this.registerForm.get('password')?.value === this.registerForm?.get('confirmPassword')) {
      const user: RegisterDTO = this.registerForm.value;
      this.userService.register(user).subscribe(
        response => {
          console.log('Registration successful', response);
          this.cookieService.set('token',response.token), this.router.navigate([''])
        },
        error => {
          alert('Registration failed'+ error);
        }
      );
    }else {
      alert('passwords do not match');
    }
  }
}
}
