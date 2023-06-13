import { UserForRegistrationDto } from '../domain/user/userForRegistrationDto';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserForAuthenticationDto } from '../domain/user/userForAuthenticationDto';
import { BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { EnvironmentUrlService } from './environment-url.service';
import { CustomEncoder } from '../common/customEncoder';
import { __param } from 'tslib';
import { ForgotPasswordDto } from '../domain/user/forgotPasswordDto';
import { ResetPasswordDto } from '../domain/user/resetPasswordDto';
import { Helpers } from '../common/helpers';
import { Result } from '../domain/response/result';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private authChangeSub = new BehaviorSubject<boolean>(false)
  public authChanged = this.authChangeSub.asObservable();

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private envUrl: EnvironmentUrlService, public helpers: Helpers) { }

  public registerUser = (route: string, body: UserForRegistrationDto) => {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }
    return this.http.post<Result>(this.createCompleteRoute(route, this.envUrl.urlAddress), body, httpOptions);
  }
  

  public resendConfirmationEmail = (route: string, email: string, uri: string) => {
    return this.http.get<Result>(this.createCompleteRoute(route + '?email=' + email + '&uri=' + uri, this.envUrl.urlAddress));
  }

  public forgotPassword = (route: string, body: ForgotPasswordDto) => {
    return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }

  public resetPassword = (route: string, body: ResetPasswordDto) => {
    return this.http.post<Result>(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }

  public loginUser = (route: string, body: UserForAuthenticationDto) => {
    return this.http.post<Result>(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }


  public logout = (route: string = "") => {
    if (!route) {
      route = "api/accounts/logout";
    }
    window.localStorage.clear();
    return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), null);
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
    return token && (!this.jwtHelper.isTokenExpired(token));
  }

  

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }

  public confirmEmail = (route: string, token: string, email: string) => {
    let params = new HttpParams({ encoder: new CustomEncoder() })
    params = params.append('token', token);
    params = params.append('email', email);
    return this.http.get(this.createCompleteRoute(route, this.envUrl.urlAddress), { params: params });
  }

  public getLoggeInUserBasicDetails = (route: string) => {
    return this.http.get(this.createCompleteRoute(route, this.envUrl.urlAddress), {});
  }

}
