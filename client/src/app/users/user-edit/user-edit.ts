import { Component, HostListener, inject, OnInit, signal, ViewChild } from '@angular/core';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account-service';
import { UsersService } from '../../_services/users-service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditor } from "../photo-editor/photo-editor";
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [TabsModule, FormsModule, PhotoEditor, TimeagoModule, DatePipe],
  templateUrl: './user-edit.html',
  styleUrl: './user-edit.css'
})
export class UserEdit implements OnInit {
  @ViewChild("editForm") editForm: NgForm | null = null;
  @HostListener("window:beforeunload", ["$event"]) notify($event:any) {
    if(this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  user = signal<User | null>(null);
  private accountService = inject(AccountService);
  private userService = inject(UsersService);
  private toaster = inject(ToastrService);

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    var user = this.accountService.currentUser();
    if(!user) return;
    this.userService.getUserByUsername(user.username).subscribe({
      next: user => this.user.set(user)
    });
  }

  updateUser() {
    this.userService.updateUser(this.editForm?.value).subscribe({
      next: () => {
        this.editForm?.reset(this.editForm.value);
        this.toaster.success("Profile updated!");
      }
    });
  }

  onUserUpdated(updated: User) {
    this.user.set(updated);
  }
}
