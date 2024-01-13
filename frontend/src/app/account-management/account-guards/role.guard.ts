import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {firstValueFrom, Observable} from 'rxjs';
import {AccountService} from "../services/account.service";

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {}

  async canActivate(route: ActivatedRouteSnapshot): Promise<boolean> {
    const expectedRole = route.data['expectedRole'];

    try {
      const userInfo = await firstValueFrom(this.accountService.whoAmI());
      const currentRole = userInfo.responseData.role;

      if (currentRole && currentRole === expectedRole) {
        return true;
      }
    } catch (error) {
      //Handle exception here
    }

    this.router.navigate(['/account/login']);
    return false;
  }

}
