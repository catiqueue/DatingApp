import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  requestCount = 0;
  private spinnerService = inject(NgxSpinnerService);

  startLoading() {
    this.requestCount++;
    this.spinnerService.show(undefined, {
      type: "cube-transition",
      bdColor: "rgba(255, 255, 255, 0)",
      color: "#333333"
    })
  }

  finishLoading() {
    this.requestCount--
    if(!this.requestCount) {
      this.spinnerService.hide();
    }
  }
}
