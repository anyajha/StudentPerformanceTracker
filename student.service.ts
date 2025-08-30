import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { enviroment } from '../enviroments/enviroments';
export interface MarksView {
 subjectName: string;
 term: string;
 marks: number;
}
export interface AttendanceView {
 date: string;      // ISO string from API
 isPresent: boolean;
}
@Injectable({ providedIn: 'root' })
export class StudentService {
 private base = `${enviroment.apiBase}/student`;
  getGrades: any;
 constructor(private http: HttpClient) {}
 getMarks(studentId: number): Observable<MarksView[]> {
   return this.http.get<MarksView[]>(`${this.base}/${studentId}/marks`);
 }
 getAttendance(studentId: number): Observable<AttendanceView[]> {
   return this.http.get<AttendanceView[]>(`${this.base}/${studentId}/attendance`);
 }
}