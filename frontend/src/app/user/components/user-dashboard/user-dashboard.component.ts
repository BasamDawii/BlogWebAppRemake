import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AccountService} from "../../../account-management/services/account.service";
import {ClientMessageService} from "../../../app-services/services/client-message.service";

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss'],
})
export class UserDashboardComponent {

  constructor(private router: Router, private accountService: AccountService, private clientMessage: ClientMessageService) { }

  async logout() {
    try {
      const response = await this.accountService.logout();
      if (response) {
        await this.router.navigate(['']);
        this.clientMessage.showSuccess(response.messageToClient!)
      } else {
        this.clientMessage.showError("Something went wrong..!")
      }
    } catch (error) {
      this.clientMessage.showError("Something went wrong..!")
    }
  }

}
