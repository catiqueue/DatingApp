import { Directive, inject, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../_services/account-service';

@Directive({
  selector: '[hasRole]', // *hasRole
  standalone: true
})
export class HasRoleDirective implements OnInit {
  @Input() hasRole: string[] = [];
  private accountService = inject(AccountService);

  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef);

  ngOnInit() {
    if(this.accountService.currentRoles().some(role => this.hasRole.includes(role)))
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    else this.viewContainerRef.clear();
  }
}
