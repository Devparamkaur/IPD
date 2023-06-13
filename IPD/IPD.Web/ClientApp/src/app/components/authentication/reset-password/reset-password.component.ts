import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Result } from '../../../domain/response/result';
import { ResetPasswordDto } from '../../../domain/user/resetPasswordDto';
import { AuthenticationService } from '../../../service/authentication.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  showSuccess: boolean;
  showError: boolean;
  errorMessage: string;
  submitted: boolean;
  private token: string;
  private email: string;

  constructor(private authService: AuthenticationService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('', [Validators.required])
    }, { validators: this.validateAreEqual });

    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
  }

  public validateControl = (controlName: string) => {
    return this.resetPasswordForm.get(controlName).invalid && this.resetPasswordForm.get(controlName).touched
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.resetPasswordForm.get(controlName).hasError(errorName)
  }
  public resetPassword = (resetPasswordFormValue) => {
    this.submitted = true;
    this.showError = this.showSuccess = false;
    const resetPass = { ...resetPasswordFormValue };
    const resetPassDto: ResetPasswordDto = {
      password: resetPass.password,
      confirmPassword: resetPass.confirm,
      token: this.token,
      email: this.email
    }
    this.authService.resetPassword('api/accounts/resetpassword', resetPassDto)
      .subscribe({
        next: (res: Result) => {
          if (res.succeeded) {
            this.showSuccess = true;
          }
        },
        error: (err: HttpErrorResponse) => {
          this.submitted = false;
          this.showError = true;
          this.errorMessage = err.error;
        }
      })
  }

  public validateAreEqual(c: AbstractControl): { notmatched: boolean } | null {
    return c.value.password === c.value.confirm ? null : { notmatched: true };
  }
}
