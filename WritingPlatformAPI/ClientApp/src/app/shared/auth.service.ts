import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";
@Injectable({
  providedIn: "root",
})
export class AuthService {
  http: HttpClient;
  url: string;
  constructor(
    http: HttpClient,
    @Inject("BASE_URL") baseUrl: string,
    private router: Router
  ) {
    this.http = http;
    this.url = baseUrl;
  }

  registerUser(register: RegisterModel) {
    return this.http.post<RegisterModel>(
      this.url + "api/auth/register",
      register
    );
  }

  loginUser(login: LoginModel) {
    return this.http.post<RegisterModel>(
      this.url + "api/auth/login",
      login
    );
  }
}

export interface LoginModel {
  username: string;
  password: string;
}
export interface RegisterModel extends LoginModel {
  email: string;
}

export class User implements RegisterModel {
  constructor() {
    this.email = "";
    this.username = "";
    this.password = "";
  }
  email: string;
  username: string;
  password: string;
}
