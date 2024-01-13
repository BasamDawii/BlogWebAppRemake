import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AccountService} from "../../services/account.service";
import {Router} from "@angular/router";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {firstValueFrom} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent{

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private clientMessage: ClientMessageService
  ) {}

  readonly loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  async submit() {
    if (this.loginForm.valid) {
      try {
        const response = await firstValueFrom(
          this.accountService.login(
            this.loginForm.value.email as string,
            this.loginForm.value.password as string
          )
        );
        this.redirectUser(response.responseData.role);
      } catch (error) {
        if (error instanceof HttpErrorResponse) {
          this.clientMessage.showError(error.error?.message)
        }
      }
    } else {
      this.clientMessage.showError('Something is not filled properly')
    }
  }

  private redirectUser(role: string) {
    switch(role) {
      case 'Admin':
        this.router.navigate(['/admin/admin-home']);
        break;
      case 'User':
        this.router.navigate(['/user/user-home']);
        break;
      default:
        this.clientMessage.showError('Role not found')
        break;
    }
  }

}
