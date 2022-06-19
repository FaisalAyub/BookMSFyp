import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs'; 
 

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> { 
    let accessToken=localStorage.getItem('authToken');
    if (accessToken) {

      request = request.clone({
      headers: request.headers
          .set('Authorization', 'Bearer ' + accessToken) 
      });
    }  
    return next.handle(request);
  }
}
