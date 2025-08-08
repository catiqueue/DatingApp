import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  var router = inject(Router);
  var toster = inject(ToastrService);

  // I hate my life
  return next(req).pipe(
    catchError(error => {
      if(error) {
        switch(error.status) {
          case 400:
            if(error.error.errors) {
              var modelStateErrors = [];
              for(var key in error.error.errors) {
                if(error.error.errors[key]) {
                  modelStateErrors.push(error.error.errors[key]);
                }
              }
              throw modelStateErrors.flat();
            }
            else {
              toster.error(error.error, error.status);
            }
            break;
          case 401:
            toster.error("Unauthorized.", error.status);
            break;
          case 404:
            router.navigateByUrl("/not-found");
            break;
          case 500:
            var navigationExtras: NavigationExtras = {
              state: {error: error.error}
            }
            router.navigateByUrl("/server-error", navigationExtras);
            break;

          default:
            toster.error("Something went horribly wrong.")
            break;
        }
      }
      throw error;
    })
  );
};
