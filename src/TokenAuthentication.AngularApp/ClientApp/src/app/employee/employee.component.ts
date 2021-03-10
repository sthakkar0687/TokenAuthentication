import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IDepartment } from '../models/idepartment';
import { DepartmentService } from '../services/department.service';
import { EmployeeService } from '../services/employee.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {
  employeeForm: FormGroup
  departments: IDepartment[];
  departmentHasError: boolean = false;
  employeeId: string = "00000000-0000-0000-0000-000000000000";
  alert: boolean = false;
  message: string = ""
  messageClass: string = ""
  constructor(private departmentService: DepartmentService,
    private employeeService: EmployeeService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private toaster: ToastrService
  ) { }

  ngOnInit() {
    this.fetchDepartments();
    this.employeeId = this._activatedRoute.snapshot.params['id'];
    this.employeeForm = new FormGroup(
      {
        employeeId: new FormControl("00000000-0000-0000-0000-000000000000", []),
        employeeName: new FormControl("", [Validators.required]),
        age: new FormControl("", [Validators.required, Validators.min(15), Validators.max(100)]),
        email: new FormControl("", [Validators.email]),
        departmentId: new FormControl("", [Validators.required]),
        gender: new FormControl("", [Validators.required]),
        subscribed: new FormControl(false, [])
      }
    )

    if (this.employeeId != undefined && this.employeeId != "") {
      let employeeDetails = this.employeeService.getEmployeeById(this.employeeId).subscribe((res: any) => {
        if (res.success) {
          //console.log(res.data)
          this.employeeForm.patchValue({
            "employeeName": res.data.employeeName,
            "age": res.data.age,
            "email": res.data.email,
            "gender": res.data.gender.toLowerCase(),
            "departmentId": res.data.departmentId != "" && res.data.departmentId != null && res.data.departmentId != "00000000-0000-0000-0000-000000000000"
              ? res.data.departmentId : "default"
          })
        }
      });

    }
    this.subscribed.valueChanges.subscribe(res => {
      if (res)
        this.email.setValidators([Validators.required, Validators.email])
      else
        this.email.setValidators([Validators.email]);
      this.email.updateValueAndValidity();
    });
  }
  get employeeName() {
    return this.employeeForm.get('employeeName')
  }
  get age() {
    return this.employeeForm.get('age')
  }
  get email() {
    return this.employeeForm.get('email')
  }
  get departmentId() {
    return this.employeeForm.get('departmentId')
  }
  get gender() {
    return this.employeeForm.get('gender')
  }
  get subscribed() {
    return this.employeeForm.get('subscribed')
  }
  fetchDepartments() {
    this.departmentService.getDepartments().subscribe((res: any) => {
      //console.log(res.data);
      this.departments = res.data
      this.employeeForm.patchValue({
        "departmentId": "default"
      })
    },

      err => { console.log(err); this.toaster.error(err.statusText, "Employees Portal", { progressBar: true }) })
  }
  private validateDepartment() {
    if (this.departmentId.value === "default" || this.departmentId.value === "")
      this.departmentHasError = true;
    else
      this.departmentHasError = false;
  }
  private saveEmployee() {
    if (this.employeeId != undefined && this.employeeId != "") {
      debugger;
      this.employeeForm.patchValue({ "employeeId": this.employeeId });
      this.employeeService.putEmployee(this.employeeId, this.employeeForm.value).subscribe((res: any) => {
        //console.log(res)
        if (res.success) {
          this._router.navigate(["/employee-list"]);
          this.toaster.success('Employee details updated successfully.', 'Employees Portal', { progressBar: true })
        }
        else
          //this.DisplayMessage(res.message,"alert-danger")
          this.toaster.error(res.message, 'Employees Portal', { progressBar: true })
      });
    }
    else {
      this.employeeForm.patchValue({ "employeeId": "00000000-0000-0000-0000-000000000000" });
      this.employeeService.postEmployee(this.employeeForm.value).subscribe((res: any) => {
        if (res.success) {
          this._router.navigate(["/employee-list"]);
          this.toaster.success('Employee added successfully.', 'Employees Portal', { progressBar: true })
        }
        else
          //this.DisplayMessage(res.message, "alert-danger")
          this.toaster.error(res.message, 'Employees Portal', { progressBar: true })
      });
    }

  }

  private DisplayMessage(message: string, messageClass: string) {
    this.alert = true;
    this.message = message;
    this.messageClass = messageClass;
    window.setTimeout(() => {
      this.alert = false;
      this.message = "";
      this.messageClass = "";
    }, 5000)
  }
}
