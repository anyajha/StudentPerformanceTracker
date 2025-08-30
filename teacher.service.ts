import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { enviroment } from '../enviroments/enviroments';
import { SubjectDto, SubjectCreateDto, MarksEntryDto, AttendanceEntryDto } from '../models/teacher.models';
@Injectable({ providedIn: 'root' })
export class TeacherService {
 private base = `${enviroment.apiBase}/teacher`;
 constructor(private http: HttpClient) {}
 addSubject(teacherId: number, body: SubjectCreateDto): Observable<SubjectDto> {
   return this.http.post<SubjectDto>(`${this.base}/${teacherId}/subject`, body);
 }
 subjects(teacherId: number): Observable<SubjectDto[]> {
   return this.http.get<SubjectDto[]>(`${this.base}/${teacherId}/subjects`);
 }
 enterMarks(teacherId: number, body: MarksEntryDto) {
   return this.http.post(`${this.base}/${teacherId}/marks`, body);
 }
 markAttendance(body: AttendanceEntryDto) {
   return this.http.post(`${this.base}/attendance`, body);
 }
}