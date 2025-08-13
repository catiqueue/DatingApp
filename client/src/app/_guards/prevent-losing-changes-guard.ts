import { CanDeactivateFn } from '@angular/router';
import { UserEdit } from '../users/user-edit/user-edit';

export const preventLosingChangesGuard: CanDeactivateFn<UserEdit> = (component) => {
  if(component.editForm?.dirty) {
    return confirm("You have some unsaved changes. Are you sure you want to leave?");
  }
  return true;
};
