import { Component, inject, OnInit, signal } from '@angular/core';
import { Register } from "../register/register";
import { SimpleUser } from '../_models/simple-user';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  registerMode = true;
  users = signal<SimpleUser[] | null>(null);
  http = inject(HttpClient);

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.http.get<SimpleUser[]>('https://localhost:5001/api/users/all').subscribe({
      next: (r) => this.users.set(r),
      error: (e) => console.error(e),
      complete: () => console.info('got all users')});
  }
}
