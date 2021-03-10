import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { IEmployee } from '../models/iemployee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private httpclient: HttpClient) { }
  private baseUrl: string = "http://localhost:54347/api/Employees/"

  getEmployees(): Observable<IEmployee[]> {
    return this.httpclient.get<IEmployee[]>(this.baseUrl);
  }
  deleteEmployee(employeeId: string): Observable<any> {
    return this.httpclient.delete(this.baseUrl + employeeId);
  }
  postEmployee(employee: IEmployee): Observable<any> {
    return this.httpclient.post(this.baseUrl, employee);
  }
  putEmployee(id: string, employee: IEmployee): Observable<any> {
    return this.httpclient.put(this.baseUrl + id, employee);
  }
  getEmployeeById(id: string): Observable<IEmployee> {
    return this.httpclient.get<IEmployee>(this.baseUrl + id);
  }
}
