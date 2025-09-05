import { Component } from '@angular/core';
import { TabsModule } from "ngx-bootstrap/tabs";
import { HasRoleDirective } from "../../_directives/has-role-directive";
import { UserManagementComponent } from "../user-management/user-management";
import { PhotoManagementComponent } from "../photo-management/photo-management";

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [TabsModule, HasRoleDirective, UserManagementComponent, PhotoManagementComponent],
  templateUrl: './admin-panel.html',
  styleUrl: './admin-panel.css'
})
export class AdminPanel {

}
