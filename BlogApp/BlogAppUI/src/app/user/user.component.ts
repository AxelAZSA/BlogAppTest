import { Component } from '@angular/core';
import { UserService } from '../Services/User/user.service';
import { User } from '../interfaces/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {
  currentUser: User | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe((user: User) => {
      this.currentUser = user;
    });
  }
}
