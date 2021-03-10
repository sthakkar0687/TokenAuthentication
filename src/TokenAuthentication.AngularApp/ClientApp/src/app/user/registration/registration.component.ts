import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IRole } from '../../models/irole';
import { AccountService } from '../../services/account.service';
import { PasswordValidator } from '../../Validation/PasswordValidator';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registrationForm: FormGroup
  roles: IRole[];
  roleHasError: boolean = false;
  constructor(private accountService:AccountService, private router:Router, private toaster: ToastrService) { }

  ngOnInit() {
    this.registrationForm = new FormGroup({
      "firstName": new FormControl("", [Validators.required]),
      "lastName": new FormControl("", [Validators.required]),
      "email": new FormControl("", [Validators.required, Validators.email]),
      "dOB": new FormControl("", [Validators.required]),
      "phoneNumber": new FormControl("", [Validators.pattern("[0-9]{10}"),Validators.required]),
      "password": new FormControl("", [Validators.required, Validators.minLength(6), Validators.maxLength(20)],),
      "confirmPassword": new FormControl("", [Validators.required],),
      "roleId": new FormControl("default",[Validators.required])
    },[PasswordValidator]);
    this.fetchRoles();
  }
  fetchRoles() {
    this.accountService.getRoles().subscribe((res: any) => {
      if (res.success)
        this.roles = res.data;
    });
  }
  get firstName() {
    return this.registrationForm.get('firstName');
  }
  get lastName() {
    return this.registrationForm.get('lastName');
  }
  get email() {
    return this.registrationForm.get('email');
  }
  get dOB() {
    return this.registrationForm.get('dOB');
  }
  get phoneNumber() {
    return this.registrationForm.get('phoneNumber');
  }
  get password() {
    return this.registrationForm.get('password');
  }
  get confirmPassword() {
    return this.registrationForm.get('confirmPassword');
  }
  get roleId() {
    return this.registrationForm.get('roleId');
  }
  registerUser() {
    
    this.accountService.createUser(this.registrationForm.value).subscribe((res: any) => {
      if (res.success) {
        this.router.navigate(["/user/login"]);
        this.toaster.success("User registered successfully.", "Employees Portal");
      }
      else
        this.toaster.success(res.message, "Employees Portal");
    });
  }
  validateRole() {
    if (this.roleId.value === "default")
      this.roleHasError = true;
    else
      this.roleHasError = false;
  }
}
