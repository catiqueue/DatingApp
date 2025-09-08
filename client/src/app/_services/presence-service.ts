import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { LoggedInUser } from '../_models/logged-in-user';
import {take} from 'rxjs';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  private hubUrl = environment.hubUrl;
  private hub?: HubConnection;
  private toster = inject(ToastrService);
  private router = inject(Router);
  public onlineUsers = signal<string[]>([]);

  public createHubConnection(user: LoggedInUser) {
    this.hub = new HubConnectionBuilder()
    .withUrl(this.hubUrl + "/presence", {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();

    this.hub.start().catch(err => this.toster.error(err));

    this.hub.on("OnSomeoneConnected", username => {
      this.onlineUsers.update(usernames => [...usernames, username]);
    });

    this.hub.on("OnSomeoneDisconnected", username => {
      this.onlineUsers.update(usernames => usernames.filter(x => x !== username));
    });

    this.hub.on("OnOnlineUsersUpdated", usernames => {
      this.onlineUsers.set(usernames);
    });

    this.hub.on("OnNewMessageReceived", ({username, knownAs}) => {
      this.toster.info(knownAs + " has sent you a new message", "New message!")
                 .onTap
                 .pipe(take(1))
                 .subscribe(() => this.router.navigateByUrl("/users/" + username + "?tab=Messages"));
    })
  }

  public closeHubConnection() {
    if(this.hub?.state === HubConnectionState.Connected)
      this.hub?.stop().catch(err => this.toster.error(err));
  }
}
