import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ILogin } from '../models/ilogin';
import { IRegister } from '../models/iregister';
import { IRole } from '../models/irole';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = "http://localhost:54347/api/"
  constructor(private httpClient: HttpClient) { }

  createUser(register: IRegister): Observable<any> {
    return this.httpClient.post(this.baseUrl + "User", register);
  }
  login(loginModel: ILogin): Observable<any> {
    return this.httpClient.post(this.baseUrl + "Auth/LoginAsync", loginModel);
  }
  getRoles(): Observable<IRole[]> {
    return this.httpClient.get<IRole[]>(this.baseUrl + "Role/GetAll");
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('expiry');
    localStorage.removeItem('email');
  }
  isLoggedIn() {
    if (localStorage.getItem('token') != null && localStorage.getItem('userId') != null
      && localStorage.getItem('email') != null && localStorage.getItem('expiry') != null) {
      let expiry :string  = localStorage.getItem('expiry');
      let differenceTime: number = (new Date(expiry).valueOf() - new Date().valueOf()) / 1000
      if (differenceTime > 0) {
        return true;
      }
      localStorage.removeItem('token');
      localStorage.removeItem('userId');
      localStorage.removeItem('expiry');
      localStorage.removeItem('email');
      return false;
    }
    return false;
  }
}
