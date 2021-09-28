import { HttpInterceptor } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ErrorInterceptorService implements HttpInterceptor {
  intercept(
    req: import('@angular/common/http').HttpRequest<any>,
    next: import('@angular/common/http').HttpHandler
  ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error) => {
        // Unauthorized Erros
        if (error.status === 401) {
          return throwError('Μη εξουσιοδοτημένος');
        }

        if (error.status === 404) {
          return throwError('Η υπηρεσία δεν βρέθηκε');
        }

        if (error.status === 400) {
          const serverError = error.error;
          const hasSuberros =
            serverError.errors && typeof serverError.errors === 'object';

          let modalStateErrors = '';

          if (hasSuberros == true) {
            for (const key in serverError.errors) {
              if (serverError.errors[key]) {
                modalStateErrors += `${serverError.errors[key]} \n`;
              }
            }
          }

          return throwError(modalStateErrors);
        }

        return throwError(
          'Internal error: Παρακαλώ επικοινωνήστε με τους διαχειριστές'
        );
      })
    );
  }
}
