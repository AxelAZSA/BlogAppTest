import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {

  const token =  inject(CookieService).get('token');
  console.log(token)
  let request = req;
  if (token) {
    request = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
    console.log('Interceptor: Authorization Header:', request.headers.get('Authorization')); // Verificar el header
  }
  return next(request);
};
