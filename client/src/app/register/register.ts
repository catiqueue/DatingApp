import { Component, inject, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account-service';
import { LoginForm } from '../_models/login-form';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  cancellationRequested = output();
  private accountService = inject(AccountService);
  registerModel: LoginForm = {
    "username": "",
    "password": ""
  };

  register() {
    if(!this.registerModel) return;
    this.accountService.register(this.registerModel).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => {
        console.error(error);
      }
    });
  }
  cancel() {
    this.cancellationRequested.emit();
  }
}
