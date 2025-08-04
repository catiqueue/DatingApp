import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account-service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { LoginForm } from '../_models/login-form';
import { App } from '../app';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
  accountService = inject(AccountService);
  loginFormModel: LoginForm = {
    "password":"",
    "username":""
  };

  login() {
    if(!this.loginFormModel) return;
    this.accountService.login(this.loginFormModel).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => {
        console.error(error);
      }
    });
  }
  logout() {
    this.accountService.logout();
  }
}
