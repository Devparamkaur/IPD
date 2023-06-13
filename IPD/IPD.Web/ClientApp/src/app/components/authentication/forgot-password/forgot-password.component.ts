import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../service/authentication.service';
import { ForgotPasswordDto } from '../../../domain/user/forgotPasswordDto';
import { EnvironmentUrlService } from '../../../service/environment-url.service';
import { Result } from '../../../domain/response/result';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
  providers: [AuthenticationService, EnvironmentUrlService],
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup
  successMessage: string;
  errorMessage: string;
  showSuccess: boolean;
  showError: boolean;
  submitted: boolean;

  constructor(private _authService: AuthenticationService, private envUrl: EnvironmentUrlService) { }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
    })
  }

  public validateControl = (controlName: string) => {
    return this.forgotPasswordForm.get(controlName).invalid && this.forgotPasswordForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.forgotPasswordForm.get(controlName).hasError(errorName)
  }

  public forgotPassword = (forgotPasswordFormValue) => {
    this.submitted = true;
    this.showError = this.showSuccess = false;
    const forgotPass = { ...forgotPasswordFormValue };

    const forgotPassDto: ForgotPasswordDto = {
      email: forgotPass.email,
      clientURI: this.envUrl.urlAddress + '/resetpassword'
    }

    this._authService.forgotPassword('api/accounts/forgotpassword', forgotPassDto)
      .subscribe({
        next: (res: Result) => {
          if (res.succeeded) {
            this.showSuccess = true;
            this.successMessage = 'The link has been sent, please check your email to reset your password.'
          }
        },
        error: (err: HttpErrorResponse) => {
          this.submitted = false;
          this.showError = true;
          this.errorMessage = err.message;
        }
      })
  }
}
