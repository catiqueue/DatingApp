import {Component, inject} from '@angular/core';
import {BsModalRef} from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [],
  templateUrl: './confirmation-dialog.html',
  styleUrl: './confirmation-dialog.css'
})
export class ConfirmationDialogComponent {
  bsModalRef = inject(BsModalRef);
  title = "";
  message = "";
  btnOkText = "";
  btnCancelText = "";
  confirmed = false;

  confirm() {
    this.confirmed = true;
    this.bsModalRef.hide();
  }

  decline() {
    this.bsModalRef.hide();
  }
}

