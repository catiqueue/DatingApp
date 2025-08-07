import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account-service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  var accountService = inject(AccountService);
  var toast = inject(ToastrService);
  if(!accountService.currentUser()) {
    // Okay, this is somewhat funny
    toast.error("You shall not pass!");
    return false;
  }
  return true;
};
