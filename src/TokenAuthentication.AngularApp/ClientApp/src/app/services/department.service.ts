import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IDepartment } from '../models/idepartment';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private baseUrl: string = "http://localhost:54347/api/Department/"
  constructor(private httpClient: HttpClient) { }

  getDepartments() : Observable<IDepartment[]> {
    return this.httpClient.get<IDepartment[]>(this.baseUrl);
  }
}
