import { Component } from '@angular/core';
import { Register } from "../register/register";

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  standalone: true,
  styleUrl: './home.css'
})
export class Home {
  registerMode = true;

  registerToggle() {
    this.registerMode = !this.registerMode;
  }
}
