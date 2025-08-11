import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css'
})
export class TestErrors {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get400Error() {
    this.http.get(this.baseUrl + "/test/bad-request").subscribe({
      next: r => console.log(r),
      error: e => console.error(e)
    });
  }

  get404Error() {
    this.http.get(this.baseUrl + "/test/not-found").subscribe({
      next: r => console.log(r),
      error: e => console.error(e)
    });
  }

  get401Error() {
    this.http.get(this.baseUrl + "/test/secret").subscribe({
      next: r => console.log(r),
      error: e => console.error(e)
    });
  }

    get500Error() {
    this.http.get(this.baseUrl + "/test/exception").subscribe({
      next: r => console.log(r),
      error: e => console.error(e)
    });
  }

  getValidationError() {
    this.http.post(this.baseUrl + "/account/register", {}).subscribe({
      next: r => console.log(r),
      error: e => {
        console.error(e);
        this.validationErrors = e;
      }
    });
  }
}
