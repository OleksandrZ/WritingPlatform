import { Component, OnInit } from "@angular/core";
import { AuthService, RegisterModel, User } from "../shared/auth.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-login-form",
  templateUrl: "./login-form.component.html",
  styleUrls: ["./login-form.component.css"],
})
export class LoginFormComponent implements OnInit {
  router: Router;
  user: User = new User();
  constructor(private auth: AuthService) {}

  ngOnInit() {}
  login(f) {
    this.auth
      .loginUser({
        username: f.username,
        password: f.password,
      })
      .subscribe(
        (data) => {
          this.router.navigate(["/"]);
        },
        (error) => {
          console.log(error.error.title);
          console.log(error.error.errors);
        }
      );
  }
}
