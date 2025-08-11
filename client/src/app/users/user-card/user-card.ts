import { Component, input } from '@angular/core';
import { User } from '../../_models/user';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-card.html',
  styleUrl: './user-card.css'
})
export class UserCard {
  user = input.required<User>();
}
