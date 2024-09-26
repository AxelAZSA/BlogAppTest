import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../Services/Category/category.service';
import { BlogEntryService } from '../Services/Blog-entry/blog-entry.service';
import { BlogEntry } from '../interfaces/blog-entry.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-edit.component.html',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  styleUrl: './blog-edit.component.css'
})
export class BlogEditComponent {
  blogForm: FormGroup;
  categories: any[] = []; // Lista de categorías
  selectedFile: File | null = null; // Archivo de imagen seleccionado

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private router:Router,
    private blogEntryService: BlogEntryService
  ) {
    // Inicializar el formulario reactivo
    this.blogForm = this.fb.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
      idCategory: [null, Validators.required],
      imageUrl: ['']
    });
  }

  ngOnInit(): void {
    // Cargar categorías desde el servicio
    this.categoryService.getAll().subscribe((data: any[]) => {
      this.categories = data;
    });
  }

  // Método para manejar la selección de archivos
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  // Método para enviar el formulario
  onSubmit(): void {
    if (this.blogForm.invalid) {
      return; // No enviar si el formulario es inválido
    }

    const blogEntry: BlogEntry = {
      idEntry: 0, // Si es nuevo, el ID es 0, puedes manejarlo según tu lógica
      title: this.blogForm.value.title,
      content: this.blogForm.value.content,
      publishDate: new Date(), // Fecha de publicación actual
      idUser: 1, // ID del usuario, puedes obtenerlo de tu sistema de autenticación
      idCategory: Number(this.blogForm.value.idCategory),
      imageUrl: '' // Este campo se llenará después de subir la imagen
    };

    // Si hay una imagen seleccionada, debes subirla primero
    if (this.selectedFile) {
      this.blogEntryService.uploadImage(this.selectedFile).subscribe(ImageUrl=> {
        console.log(ImageUrl);
        blogEntry.imageUrl = ImageUrl.imageUrl.toString(); // Asigna la URL de la imagen
        console.log(blogEntry)
        this.saveBlogEntry(blogEntry); // Guarda la entrada del blog después de subir la imagen
      });
    } else {
      this.saveBlogEntry(blogEntry); // Guarda la entrada del blog directamente
    }
  }

  // Método para guardar la entrada del blog
  private saveBlogEntry(blogEntry: BlogEntry): void {
    this.blogEntryService.create(blogEntry).subscribe(response => {
      console.log('Blog entry saved successfully');
      this.router.navigate(['']);
      // Aquí puedes redirigir o mostrar un mensaje de éxito
    });
  }
}
