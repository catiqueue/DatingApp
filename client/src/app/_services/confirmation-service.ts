import {inject, Injectable} from '@angular/core';
import {BsModalRef, BsModalService, ModalOptions} from 'ngx-bootstrap/modal';
import {ConfirmationDialogComponent} from '../modals/confirmation-dialog/confirmation-dialog';
import {map} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationService {
  private modalService = inject(BsModalService);
  bsModalRef?: BsModalRef;

  confirm(message: string, title = "Confirmation", btnOkText = 'Proceed', btnCancelText = 'Cancel') {
    const config: ModalOptions = {
      initialState: {title, message, btnOkText, btnCancelText}
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, config);
    return this.bsModalRef.onHidden?.pipe(map(() => this.bsModalRef?.content.confirmed ?? false));
  }
}
