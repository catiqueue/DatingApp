import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('DatingApp');
  http = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/users/all').subscribe({
      next: (r) => this.users = r,
      error: (e) => console.error(e),
      complete: () => console.info('got all users')});
  }
}
