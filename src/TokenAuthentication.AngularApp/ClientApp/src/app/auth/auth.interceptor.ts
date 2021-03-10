import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private toaster: ToastrService, private router: Router, private activatedRoute: ActivatedRoute) { }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    let token = localStorage.getItem('token');
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        }
      });
    }
    return next.handle(request).pipe(
      //retry(2),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // 401 handled in auth.interceptor
          this.toaster.error(error.message);
        }
        else if (error.status === 403) {

          this.router.navigate(['home'], { relativeTo: this.activatedRoute });
          this.toaster.error('You are not authorized to access the form', "Employees Portal");
        }
        else { return throwError(error); }
      })
    );
  }
}
