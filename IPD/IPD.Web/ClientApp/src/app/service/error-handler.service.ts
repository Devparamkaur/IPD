import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthenticationService } from './authentication.service';
@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService implements HttpInterceptor {

  constructor(private router: Router, private authService: AuthenticationService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.handleError(error);
          return throwError(() => new Error(errorMessage));
        })
      )
  }

  private handleError = (error: HttpErrorResponse): string => {
    console.log(error);
    if (error.status === 404) {
      return this.handleNotFound(error);
    }
    else if (error.status === 400) {
      return this.handleBadRequest(error);
    } else if (error.status === 401) {
      return this.handleBadRequest(error);
    }
    return error.message;
  }
  private handleNotFound = (error: HttpErrorResponse): string => {
    //this.router.navigate(['/404']);
    return error.message;
  }

  private handleUnAuthorzied = (error: HttpErrorResponse): string => {
    // this.router.navigate(['/not-authorized']);
    return error.message;
  }

  private handleBadRequest = (error: HttpErrorResponse): string => {
    console.log(this.router.url);
    if (this.router.url === '/register') {
      let message = '';
      const values = Object.values(error.error);
      values.map((m: string) => {
        message += m + '<br>';
      });
      return message.slice(0, -4);
    }
    else {
      return error.error ? error.error : error.message;
    }
  }
}
