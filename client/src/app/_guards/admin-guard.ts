import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account-service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  var accountService = inject(AccountService);
  var toaster = inject(ToastrService);

  if(accountService.currentRoles().includes("Admin") ||
     accountService.currentRoles().includes("Moderator")) {
    return true;
  } else {
    toaster.error("You have no permission to go here.");
    return false;
  }
};
