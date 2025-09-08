import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private requestCount = 0;
  private spinnerService = inject(NgxSpinnerService);

  public startLoading() {
    this.requestCount++;
    this.spinnerService.show(undefined, {
      type: "cube-transition",
      bdColor: "rgba(255, 255, 255, 0)",
      color: "#333333",
    })
  }

  public finishLoading() {
    this.requestCount--
    if(!this.requestCount) {
      this.spinnerService.hide();
    }
  }
}
