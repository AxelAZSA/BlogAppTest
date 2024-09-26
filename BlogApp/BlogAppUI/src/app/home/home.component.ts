import { Component } from '@angular/core';
import { BlogEntryService } from '../Services/Blog-entry/blog-entry.service';
import { BlogEntry } from '../interfaces/blog-entry.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  entries: BlogEntry[]=[];
  constructor(private blogService: BlogEntryService) {}

  ngOnInit(): void {
    this.blogService.getAll().subscribe((entries: BlogEntry[]) => {
      if (entries.length > 0) {
        this.entries = entries; // Asigna la primera entrada del blog
      }
    });
  }
}
