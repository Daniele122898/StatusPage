import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {JwtHelperService} from '@auth0/angular-jwt';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs/operators';
import {LoginResponse} from '../models/LoginResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authSubj = new BehaviorSubject<void>(null);
  public AuthObs = this.authSubj.asObservable();

  private baseUrl = environment.apiUrl + 'auth/';
  private jwtHelper = new JwtHelperService();
  private decodedToken: any;

  constructor(
    private http: HttpClient
  ) { }

  public loggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  public login(username: string, password: string): Observable<any> {
    return this.http.post(this.baseUrl + 'login', {username, password})
      .pipe(
        map((resp: any) => {
          const loginResp: LoginResponse = resp;
          if (loginResp) {
            localStorage.setItem('token', loginResp.token);
            this.decodedToken = this.jwtHelper.decodeToken(loginResp.token);
            this.authSubj.next(null);
          }
        })
      );
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.decodedToken = null;
    this.authSubj.next(null);
  }
}
