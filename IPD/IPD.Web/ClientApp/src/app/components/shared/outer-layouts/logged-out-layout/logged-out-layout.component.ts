import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Helpers } from '../../../../common/helpers';
import { AuthenticationService } from '../../../../service/authentication.service';

@Component({
  selector: 'app-logged-out-layout',
  templateUrl: './logged-out-layout.component.html',
  styleUrls: ['./logged-out-layout.component.css'],
  animations: [
  ]
})
export class LoggedOutLayoutComponent implements OnInit {

  public isUserAuthenticated: boolean;

  constructor(private helpers: Helpers, private authService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.isUserAuthenticated = this.authService.isUserAuthenticated();
  }

  logOut() {
    this.authService.logout().subscribe({
      next: (res) => {
        this.router.navigate(['/login']);
      },
      error: (err: HttpErrorResponse) => {
      }
    });
  }

}
