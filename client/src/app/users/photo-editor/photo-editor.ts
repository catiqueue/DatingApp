import { Component, inject, input, OnInit, output } from '@angular/core';
import { User } from '../../_models/user';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account-service';
import { environment } from '../../../environments/environment';
import { pipe } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { Photo } from '../../_models/photo';
import { UsersService } from '../../_services/users-service';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.html',
  styleUrl: './photo-editor.css'
})
export class PhotoEditor implements OnInit {
  private accountService = inject(AccountService);
  private usersService = inject(UsersService);

  user = input.required<User>();
  userUpdated = output<User>();

  uploader?: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;

  ngOnInit(): void {
    this.initUploader();
  }

  onFileOverBase(event: any) {
    this.hasBaseDropzoneOver = event;
  }

  setMainPhoto(photo: Photo) {
    this.usersService.setMainPhoto(photo).subscribe({
      next: () => {
        // this feels so stupid
        this.accountService.setAvatar(photo);

        var updatedUser =  {...this.user()}
        updatedUser.avatarUrl = photo.url;

        updatedUser.photos.forEach(ph => {
          if(ph.isMain) ph.isMain = false;
          if(ph.id == photo.id) ph.isMain = true;
        });

        this.userUpdated.emit(updatedUser);
      }
    })
  }

  deletePhoto(photo: Photo) {
    this.usersService.deletePhoto(photo).subscribe({
      next: () => {
        var updatedUser = {...this.user()};
        updatedUser.photos = updatedUser.photos.filter(ph => ph.id !== photo.id);
        this.userUpdated.emit(updatedUser);
      }
    });
  }

  initUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + "/users/add-photo",
      authToken: "Bearer" + " " + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ["image"],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 6 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }
    this.uploader.onSuccessItem = (file, response, status, headers) => {
      var photo = JSON.parse(response) as Photo;
      var updatedUser = {...this.user()};
      updatedUser.photos.push(photo);
      if(photo.isMain) {
        this.accountService.setAvatar(photo);
        updatedUser.avatarUrl = photo.url;
      }
      this.userUpdated.emit(updatedUser);
    }
  }
}
