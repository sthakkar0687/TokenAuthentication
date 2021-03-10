import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup
  roleHasError: boolean = false;
  constructor(private accountService: AccountService, private router: Router, private toaster: ToastrService) { }

  ngOnInit() {
    this.loginForm = new FormGroup({
      "userName": new FormControl("", [Validators.required, Validators.email]),
      "password": new FormControl("", [Validators.required, Validators.minLength(6), Validators.maxLength(20)],),
    });
    if (localStorage.getItem('token') != null) {
      //alert('logged-in')
      this.router.navigateByUrl['/employee-list']
    }
  }


  get userName() {
    return this.loginForm.get('userName');
  }

  get password() {
    return this.loginForm.get('password');
  }

  loginUser() {

    this.accountService.login(this.loginForm.value).subscribe((res: any) => {
      //console.log(res);
      if (res.success) {
        localStorage.setItem('userId', res.data.userId);
        localStorage.setItem('token', res.data.token);
        localStorage.setItem('email', res.data.email);
        localStorage.setItem('expiry', res.data.expiry);
        this.router.navigate(["/employee-list"]);
        this.toaster.success('You have logged in successfully.', 'Login Successful', { progressBar: true });
      }
      else
        this.toaster.error(res.message, 'Login failure', { progressBar: true });
    });
  }
}
