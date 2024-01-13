import { Component, OnInit } from '@angular/core';
import {User} from "../../../app-services/app-models/AccountModels";
import {AccountService} from "../../../account-management/services/account.service";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {AlertController} from "@ionic/angular";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {
  users: User[] | undefined = [];

  constructor(
    private accountService: AccountService,
    private clientMessageService: ClientMessageService,
    private alertController: AlertController
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.accountService.getAllUsers().subscribe({
      next: (response) => {
        this.users = response.responseData;
        if (response.messageToClient) {
          this.clientMessageService.showSuccess(response.messageToClient);
        }
      },
      error: (error) => {
        this.clientMessageService.showError(error.error.messageToClient || 'Error fetching users');
      }
    });
  }

  async deleteUser(userId: number) {
    const alert = await this.alertController.create({
      header: 'Confirm Delete',
      message: 'Are you sure you want to delete this user?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: (blah) => {
            console.log('Delete cancelled');
          }
        }, {
          text: 'Yes, Delete',
          handler: () => {
            this.performDeletion(userId);
          }
        }
      ]
    });

    await alert.present();
  }

  private performDeletion(userId: number) {
    this.accountService.deleteUser(userId).subscribe({
      next: (response) => {
        this.loadUsers();
        this.clientMessageService.showSuccess(response.messageToClient || 'User deleted successfully');
      },
      error: (error) => {
        this.clientMessageService.showError(error.error.messageToClient || 'Error deleting user');
      }
    });
  }
}
