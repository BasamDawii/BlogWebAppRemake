import { Injectable } from '@angular/core';
import {ResponseDto} from "../../app-services/app-models/ResponseModels";
import {User} from "../../app-services/app-models/AccountModels";
import {firstValueFrom, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly baseUrl = environment.apiUrl + '/api/account/';

  constructor(private http : HttpClient) { }

  login(email: string, password: string): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.baseUrl + 'login', { email, password }, {withCredentials : true});
  }

  whoAmI(): Observable<ResponseDto<any>> {
    return this.http.get<ResponseDto<any>>(this.baseUrl + 'whoami', {withCredentials : true});
  }

  async logout(): Promise<ResponseDto<any>> {
    return await firstValueFrom(this.http.post<ResponseDto<User>>(this.baseUrl + 'logout', {}));
  }

  register(userData: any): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(this.baseUrl + 'register', userData);
  }

  getAllUsers(): Observable<ResponseDto<User[]>> {
    return this.http.get<ResponseDto<User[]>>(this.baseUrl + 'all-users', { withCredentials: true });
  }

  deleteUser(userId: number): Observable<ResponseDto<any>> {
    return this.http.delete<ResponseDto<any>>(this.baseUrl + 'delete-user/' + userId, { withCredentials: true });
  }
}
