import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { CounterComponent } from './components/counter/counter.component';
import { FetchDataComponent } from './components/fetch-data/fetch-data.component';
import { LoginComponent } from './components/authentication/login/login.component';
import { RegisterComponent } from './components/authentication/register/register.component';
import { EmailConfirmationComponent } from './components/authentication/email-confirmation/email-confirmation.component';
import { ConfirmationEmailSentComponent } from './components/authentication/confirmation-email-sent/confirmation-email-sent.component';
import { ForgotPasswordComponent } from './components/authentication/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/authentication/reset-password/reset-password.component';
import { LoggedOutLayoutComponent } from './components/shared/outer-layouts/logged-out-layout/logged-out-layout.component';
import { LoggedInGuard } from './guards/logged-in.guard';
import { Helpers } from './common/helpers';
import { ErrorHandlerService } from './service/error-handler.service';
import { UserLayoutComponent } from './components/shared/inner-layouts/user-layout/user-layout.component';
import { AuthGuard } from './guards/auth.guard';
import { JwtModule } from '@auth0/angular-jwt';
import { NavBreadcrumbsComponent } from './components/shared/inner-layouts/nav-breadcrumbs/nav-breadcrumbs.component';
import { NavSidebarComponent } from './components/shared/inner-layouts/nav-sidebar/nav-sidebar.component';

export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    RegisterComponent,
    EmailConfirmationComponent,
    ConfirmationEmailSentComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    LoggedOutLayoutComponent,
    UserLayoutComponent,
    NavBreadcrumbsComponent,
    NavSidebarComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([

      /* Logged-Out User Routes */
      {
        path: '',
        component: LoggedOutLayoutComponent,
        children: [
          { path: 'login', component: LoginComponent, title: 'Sign In', canActivate: [LoggedInGuard] },
          { path: 'register', component: RegisterComponent, title: 'Register', canActivate: [LoggedInGuard] },
          { path: 'emailconfirmation', component: EmailConfirmationComponent },
          { path: 'confirmation-email-sent', component: ConfirmationEmailSentComponent },
          { path: 'forgot-password', component: ForgotPasswordComponent },
          { path: 'resetpassword', component: ResetPasswordComponent },
        ]
      },

      /* Logged-In User Routes */
      {
        path: '',
        component: UserLayoutComponent,
        canActivate: [AuthGuard],
        children: [
          { path: 'home', component: HomeComponent, title: 'Home' },
          { path: '', component: HomeComponent, pathMatch: 'full' },
          { path: 'counter', component: CounterComponent },
          { path: 'fetch-data', component: FetchDataComponent },
        ]
      },
      
    ],
      {
        scrollPositionRestoration: 'enabled'
      }),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7238"],
        //disallowedRoutesRoutes: []
      }
    })
  ],
  providers: [Helpers, {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorHandlerService,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
