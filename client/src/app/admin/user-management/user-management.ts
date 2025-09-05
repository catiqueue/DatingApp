import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin-service';
import { User } from '../../_models/user';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [],
  templateUrl: './user-management.html',
  styleUrl: './user-management.css'
})
export class UserManagementComponent implements OnInit {
  private adminService = inject(AdminService);
  private modalService = inject(BsModalService);
  protected users: User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();

  ngOnInit(): void {
    this.loadUsersWithRoles();
  }

  openRolesModal(user: User) {
    var initialState: ModalOptions = {
      class: "modal-lg",
      initialState: {
        title: "User roles",
        existingRoles: ["Admin", "Moderator", "User"],
        username: user.userName,
        selectedRoles: [...user.roles],
        rolesUpdated: false
      },
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, initialState);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        if(this.bsModalRef.content?.rolesUpdated) {
          this.adminService.updateUserRoles(user.userName, this.bsModalRef.content.selectedRoles).subscribe({
            next: roles => user.roles = roles
          });
        }
      }
    });
  }

  loadUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe({
      next: response => this.users = response
    });
  }
}
