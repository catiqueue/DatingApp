import { CanDeactivateFn } from '@angular/router';
import { UserEdit } from '../users/user-edit/user-edit';
import {ConfirmationService} from '../_services/confirmation-service';
import {inject} from '@angular/core';

export const preventLosingChangesGuard: CanDeactivateFn<UserEdit> = (component) => {
  const confirmationService = inject(ConfirmationService);
  if(component.editForm?.dirty) {
    return confirmationService.confirm("Are you sure you want to continue? Any unsaved changes will be lost.") ?? false;
  }
  return true;
};
