import { Component, OnInit } from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {Router} from "@angular/router";
import {AccountService} from "../../services/account.service";
import {FormBuilder, Validators} from "@angular/forms";
import {CustomValidators} from "../../../app-services/services/CustomValidators";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent{
  readonly form = this.fb.group({
    fullName: [null, Validators.required],
    email: [null, [Validators.required, Validators.email]],
    password: [null, [Validators.required, Validators.minLength(8)]],
    passwordRepeat: [null, [Validators.required, CustomValidators.matchOther('password')]],
  });

  constructor(private fb: FormBuilder,
              private service : AccountService,
              private router : Router,
              private clientMessage: ClientMessageService
  ) { }

  async submit() {
    if (this.form.valid) {
      try {
        const response = await firstValueFrom(this.service.register(this.form.value));
        if(response) {
          this.clientMessage.showSuccess(response.messageToClient!);
          this.router.navigate(['/login'])
        }
      } catch (e) {
        if (e instanceof HttpErrorResponse) {
          this.clientMessage.showError(e.error.messageToClient);
        } else {
          this.clientMessage.showError('An unexpected error occurred');
        }
      }
    } else {
      this.clientMessage.showWarning('Some fields are not populated properly');
    }
  }
}
