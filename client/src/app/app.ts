import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Nav } from "./nav/nav";
import { AccountService } from './_services/account-service';
import { RouterOutlet } from '@angular/router';
import { NgxSpinnerComponent } from "ngx-spinner";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Nav, NgxSpinnerComponent],
  templateUrl: './app.html',
  standalone: true,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('DatingApp');
  private accountService = inject(AccountService);
  http = inject(HttpClient);


  ngOnInit(): void {
    this.accountService.loadCurrentUser();
  }
}
