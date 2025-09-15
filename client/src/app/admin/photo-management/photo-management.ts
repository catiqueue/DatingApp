import {Component, inject, OnInit} from '@angular/core';
import {AdminService} from '../../_services/admin-service';
import {PaginationComponent} from 'ngx-bootstrap/pagination';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-photo-management',
  standalone: true,
  imports: [
    PaginationComponent,
    FormsModule
  ],
  templateUrl: './photo-management.html',
  styleUrl: './photo-management.css'
})
export class PhotoManagementComponent implements OnInit {
  protected adminService = inject(AdminService);
  dirty = false;

  ngOnInit() {
    this.loadPhotosToModerate();
  }

  loadPhotosToModerate() {
    this.adminService.loadPhotosForModeration();
  }

  approvePhoto(photoId: number) {
    this.dirty = true;
    this.adminService.approvePhoto(photoId).subscribe({
      next: () => this.adminService.photos.update(prev => prev.filter(p => p.id !== photoId))
    });
  }

  rejectPhoto(photoId: number) {
    this.dirty = true;
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () => this.adminService.photos.update(prev => prev.filter(p => p.id !== photoId))
    });
  }

  onPageChanged(event: any) {
    const pagination = this.adminService.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    // if the page was manipulated on, then I don't want to load the next page, as it would skip some of the photos.
    // but we can go backwards no problem.
    if(!this.dirty || pagination.current.pageNumber > event.page) {
      pagination.current.pageNumber = event.page;
      this.adminService.pagination.set(pagination);
    }
    this.adminService.loadPhotosForModeration();
    this.dirty = false;
  }
}
