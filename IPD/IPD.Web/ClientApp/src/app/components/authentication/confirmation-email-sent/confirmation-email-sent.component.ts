import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Result } from '../../../domain/response/result';
import { AuthenticationService } from '../../../service/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-confirmation-email-sent',
  templateUrl: './confirmation-email-sent.component.html',
  styleUrls: ['./confirmation-email-sent.component.css']
})
export class ConfirmationEmailSentComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private authService: AuthenticationService, private toastrService: ToastrService) {
  }

  emailId: string;
  clientUri: string;
  ngOnInit() {
    this.emailId = this.activatedRoute.snapshot.queryParams['token'];
    this.clientUri = this.activatedRoute.snapshot.queryParams['email'];
  }

  resendVerificationEmail() {
    this.authService.resendConfirmationEmail("api/accounts/resendConfirmationEmail", this.emailId, this.clientUri)
      .subscribe({
        next: (res: Result) => {
          if (res.succeeded) {
            this.toastrService.success('Please verify your email from your account');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.toastrService.error('Something went wrong!');
        }
      });
  }
}
