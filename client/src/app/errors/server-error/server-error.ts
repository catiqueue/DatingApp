import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css'
})
export class ServerError {
  error: any;
  constructor(private router: Router) {
    var navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.["error"];

  }
}
