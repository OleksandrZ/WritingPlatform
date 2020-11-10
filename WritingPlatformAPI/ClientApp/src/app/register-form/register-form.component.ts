import { Component, Inject, OnInit } from "@angular/core";
import { RegisterModel } from "../shared/auth.service";
import { AuthService } from "./../shared/auth.service";
import { Router } from "@angular/router";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";

@Component({
  selector: "app-register-form",
  templateUrl: "./register-form.component.html",
  styleUrls: ["./register-form.component.css"],
})
export class RegisterFormComponent implements OnInit {
  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit() {}

  registerUser(f) {
    this.auth
      .registerUser({
        username: f.username,
        email: f.email,
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
