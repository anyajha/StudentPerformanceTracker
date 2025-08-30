import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { enviroment } from '../enviroments/enviroments';
import { Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class AdminService {
 private base = `${enviroment.apiBase}/admin`;
 constructor(private http: HttpClient) {}
 studentsReport(): Observable<any[]> { return this.http.get<any[]>(`${this.base}/students-report`); }
 teachersReport(): Observable<any[]> { return this.http.get<any[]>(`${this.base}/teachers-report`); }
 studentsCategories(): Observable<any> { return this.http.get<any>(`${this.base}/students-categories`); }
}