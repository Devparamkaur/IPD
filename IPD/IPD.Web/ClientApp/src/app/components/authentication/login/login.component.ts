import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Helpers } from '../../../common/helpers';
import { AuthenticationService } from '../../../service/authentication.service';
import { UserForAuthenticationDto } from '../../../domain/user/userForAuthenticationDto';
import { Result } from '../../../domain/response/result';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [AuthenticationService, ToastrService],
  animations: [

  ]
})
export class LoginComponent {

  private returnUrl: string;

  loginForm: FormGroup;

  submitted: boolean;

  constructor(
    public helpers: Helpers, private authService: AuthenticationService, private toastrService: ToastrService, private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit() {

    this.loginForm = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required])
    })
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/home';
  }


  validateControl = (controlName: string) => {
    return this.loginForm.get(controlName).invalid && this.loginForm.get(controlName).touched
  }
  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.get(controlName).hasError(errorName)
  }

  loginUser = (loginFormValue) => {
    this.submitted = true;
    const login = { ...loginFormValue };
    const userForAuth: UserForAuthenticationDto = {
      email: login.email,
      password: login.password
    }
    this.authService.loginUser('api/accounts/login', userForAuth)
      .subscribe({
        next: (res: Result) => {
          if (res.succeeded) {
            localStorage.setItem("token", res.data.token);


            this.router.navigate([this.returnUrl]);
            this.authService.sendAuthStateChangeNotification(res.succeeded);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.submitted = false;
          this.toastrService.error(err.message);
        }
      })
  }

}
