import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Nav } from "./nav/nav";
import { AccountService } from './_services/account-service';
import { LoggedInUser } from './_models/logged-in-user';
import { RouterOutlet } from '@angular/router';
import { NgxSpinnerComponent } from "ngx-spinner";
import { LikesService } from './_services/likes-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Nav, NgxSpinnerComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('DatingApp');
  private accountService = inject(AccountService);
  private likesService = inject(LikesService);
  http = inject(HttpClient);


  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(): void {
    var userString = localStorage.getItem("user");
    if(!userString) return;
    var user:LoggedInUser = JSON.parse(userString);
    this.accountService.currentUser.set(user);
    this.likesService.loadLikedIds();
  }
}
