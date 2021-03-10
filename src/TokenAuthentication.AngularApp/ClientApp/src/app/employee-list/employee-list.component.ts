import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IEmployee } from '../models/iemployee';
import { EmployeeService } from '../services/employee.service';

@Component({
  selector: 'employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  all: number = 0;
  male: number = 0;
  female: number = 0;
  alert: boolean = false;
  message: string = ""
  messageClass: string = ""
  selectedValue :string = "all"
  constructor(private employeeService: EmployeeService, private toaster: ToastrService) { }
  employees: IEmployee[];
  ngOnInit() {
    window.setTimeout(() => {
      this.fetchEmployees();
    }, 300);
    
  }

  private fetchEmployees(): void {
    this.employeeService.getEmployees().subscribe(
      (res: any) => {        
        this.employees = res.data;
        this.all = res.data.length;
        this.male = res.data.filter(p => p.gender.toLowerCase() == "male").length
        this.female = res.data.filter(p => p.gender.toLowerCase() == "female").length        
      }
      , err => { console.log(err); this.toaster.error(err.statusText, "Employees Portal", { progressBar: true }) }
    );
  }
  private deleteEmployee(employeeId: string, employeeName: string): void {
    if (confirm("Do want to delete the employee  - " + employeeName))
      this.employeeService.deleteEmployee(employeeId).subscribe((res: any) => {
        if (res.success) {
          //this.DisplayMessage(res.message, "alert-success");
          this.toaster.success(res.message, "Employees Portal", { progressBar: true })
        }
        else {
          //this.DisplayMessage(res.message, "alert-danger");
          this.toaster.error(res.message, "Employees Portal", {progressBar:true})
        }
        this.fetchEmployees();
      })
  }

  private DisplayMessage(message: string, messageClass: string) {
    this.alert = false;
    this.message = message;
    this.messageClass = messageClass;
    window.setTimeout(() => {
      this.alert = false;
      this.message = "";
      this.messageClass = "";
    }, 5000)
  }
  private trackByEmployeeId(index: number, employee: IEmployee) {
    return employee.employeeId;
  }

  onRadioButtonChanged(selectedValue: string) {
    //console.log(selectedValue)
    this.selectedValue = selectedValue;
  }
}
