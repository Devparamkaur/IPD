import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Helpers } from '../../../common/helpers';
import { AuthenticationService } from '../../../service/authentication.service';
import { EnvironmentUrlService } from '../../../service/environment-url.service';
import {  UserForRegistrationDto } from '../../../domain/user/userForRegistrationDto';
import { Result } from '../../../domain/response/result';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [AuthenticationService, ToastrService, EnvironmentUrlService],
  animations: [
  ]
})
export class RegisterComponent {

  basicInfoForm: FormGroup;

  constructor(private activatedRoute: ActivatedRoute, private authService: AuthenticationService,
    private messageService: ToastrService,
    public helpers: Helpers, private router: Router, private envUrl: EnvironmentUrlService) {
  }

  ngOnInit() {

    this.activatedRoute.queryParams.subscribe(params => {


    });


  }

  public validateControl = (controlName: string) => {
    let form = this.basicInfoForm;
    return form.get(controlName).invalid && form.get(controlName).touched
  }
  public hasError = (controlName: string, errorName: string) => {
    let form = this.basicInfoForm;
    return form.get(controlName).hasError(errorName)
  }
  public registerUser = (basicInfoFormValue) => {
    const formValues = { ...basicInfoFormValue };
    const user: UserForRegistrationDto = {
      firstName: formValues.firstName,
      lastName: formValues.lastName,
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.confirmPassword,
      clientURI: this.envUrl.urlAddress + '/emailconfirmation',
    };

    this.authService.registerUser("api/accounts/BasicInfo", user)
      .subscribe({
        next: (res: Result) => {
          if (res.succeeded) {
            this.messageService.success('Please verify your email from your account');
            this.router.navigate(['/confirmation-email-sent', { email: user.email, uri: user.clientURI }]);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.messageService.error(err.message);
        }
      });
  }


  showResponse(response) {
    //call to a backend to verify against recaptcha with private key
  }


  public validateAreEqual(c: AbstractControl): { notmatched: boolean } | null {
    return c.value.password === c.value.confirmPassword ? null : { notmatched: true };
  }


}


