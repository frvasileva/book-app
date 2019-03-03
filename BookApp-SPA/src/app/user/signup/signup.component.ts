import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { AuthService } from "src/app/_services/auth.service";
import { Router } from "@angular/router";
import { AlertifyService } from "src/app/_services/alertify.service";

@Component({
  selector: "app-signup",
  templateUrl: "./signup.component.html",
  styleUrls: ["./signup.component.scss"]
})
export class SignupComponent implements OnInit {
  signUpForm: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.signUpForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [
        Validators.required,
        Validators.minLength(6)
      ]),
      userAgreementChecked: new FormControl(null, Validators.requiredTrue)
    });
  }

  onSubmit() {
    console.log(this.signUpForm.value);

    this.authService.reigster(this.signUpForm.value).subscribe(
      next => {
        this.alertify.success("sucessfull registration!");
      },
      error => {
        this.alertify.error("Failed to register");
      }
    );
  }
}
