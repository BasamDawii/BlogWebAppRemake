import { Component, OnInit } from '@angular/core';
import {Category} from "../../../app-services/app-models/BlogModels";
import {BlogService} from "../../../app-services/services/blog.service";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {AlertController} from "@ionic/angular";

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
})
export class CategoriesComponent implements OnInit {
  categories: Category[] = [];

  constructor(
    private blogService: BlogService,
    private clientMessage: ClientMessageService,
    private alertController: AlertController
  ) {}

  ngOnInit() {
    this.blogService.getAllCategories().subscribe({
      next: (response) => {
        if (response.responseData) {
          this.categories = response.responseData;
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
      }
    });
  }

  async createCategory() {
    const alert = await this.alertController.create({
      header: 'Create New Category',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'Category Name'
        }
      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel'
        },
        {
          text: 'Create',
          handler: (data) => {
            this.blogService.createCategory({ name: data.name }).subscribe({
              next: (response) => {
                this.clientMessage.showInfo(response.messageToClient!);
                this.refreshCategories();
              },
              error: (err) => this.clientMessage.showError(err.error?.messageToClient)
            });
          }
        }
      ]
    });

    await alert.present();
  }


  async updateCategory(category: Category) {
    const alert = await this.alertController.create({
      header: 'Update Category',
      inputs: [
        {
          name: 'name',
          type: 'text',
          value: category.name,
          placeholder: 'Category Name'
        }
      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel'
        },
        {
          text: 'Update',
          handler: (data) => {
            this.blogService.updateCategory({ id: category.id, name: data.name }).subscribe({
              next: (response) => {
                this.clientMessage.showInfo(response.messageToClient!);
                this.refreshCategories();
              },
              error: (err) => this.clientMessage.showError(err.error?.messageToClient)
            });
          }
        }
      ]
    });

    await alert.present();
  }


  deleteCategory(categoryId: number) {
    this.blogService.deleteCategory(categoryId).subscribe({
      next: (response) => {
        this.clientMessage.showInfo(response.messageToClient!);
        this.refreshCategories();
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
      }
    });
  }

  refreshCategories() {
    this.blogService.getAllCategories().subscribe({
      next: (response) => {
        if (response.responseData) {
          this.categories = response.responseData;
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
      }
    });
  }

}

