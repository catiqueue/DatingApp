import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  standalone: true,
  imports: [],
  templateUrl: './roles-modal.html',
  styleUrl: './roles-modal.css'
})
export class RolesModalComponent {
  bsModalRef = inject(BsModalRef);
  username = "";
  title = "";
  existingRoles: string[] = [];
  selectedRoles: string[] = [];
  rolesUpdated: boolean = false;

  toggleRole(toggledRole: string) {
    if(this.selectedRoles.includes(toggledRole))
      this.selectedRoles = this.selectedRoles.filter(selectedRole => selectedRole !== toggledRole);
    else this.selectedRoles.push(toggledRole);
  }

  onRolesSelected() {
    this.rolesUpdated = true;
    this.bsModalRef.hide();
  }
}
