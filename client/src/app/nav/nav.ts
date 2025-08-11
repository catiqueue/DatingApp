import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account-service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { LoginForm } from '../_models/login-form';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-nav',
  imports: [RouterLink, RouterLinkActive, FormsModule, BsDropdownModule, TitleCasePipe],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  loginFormModel: LoginForm = {
    "password":"",
    "username":""
  };

  login() {
    if(!this.loginFormModel) return;
    this.accountService.login(this.loginFormModel).subscribe({
      next: _ => {
        this.router.navigateByUrl("/users");
      },
      // едет error через error, видит error в error error. сунул error error в error, error error error error
      error: error => {
        this.toastr.error(error.error);
      }
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/");
  }
}
