import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { EmployeeComponent } from './employee/employee.component';
import { EmployeeFilterComponent } from './employee-filter/employee-filter.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthGuard } from './auth/auth.guard';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AuthInterceptor } from './auth/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    EmployeeListComponent,
    EmployeeComponent,
    EmployeeFilterComponent,
    UserComponent,
    LoginComponent,
    RegistrationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularFontAwesomeModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    RouterModule.forRoot([
      //{ path: '', component:HomeComponent, pathMatch: 'full' },
      { path: '', redirectTo: 'user/login', pathMatch: 'full' },
      { path: 'home', component:HomeComponent  },
      { path: 'employee-list', component: EmployeeListComponent, canActivate: [AuthGuard]},
      { path: 'employee/add', component: EmployeeComponent, canActivate: [AuthGuard] },
      { path: 'employee/edit/:id', component: EmployeeComponent, canActivate: [AuthGuard] },
      { path: 'fetch-data', component: FetchDataComponent },
      //{ path: 'login', component: LoginComponent },
      //{ path: 'register', component: RegistrationComponent }, 
      {
        path: 'user', component: UserComponent, 
        children: [          
          { path: 'login', component: LoginComponent },
          { path: 'register', component: RegistrationComponent },          
        ]
      }
    ])
  ],
  providers: [
//{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
