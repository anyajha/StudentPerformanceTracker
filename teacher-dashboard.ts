import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TeacherService } from '../../services/teacher.service';

import { AuthService } from '../../services/auth.services';
import { SubjectDto } from '../../models/teacher.models';
import { MatOption } from '@angular/material/select';
@Component({
 standalone: true,
 selector: 'app-teacher-dashboard',
 imports: [
   CommonModule, FormsModule, ReactiveFormsModule,
   MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,MatListModule, MatSnackBarModule
 ],
 templateUrl: './teacher-dashboard.html',
 
})
export class TeacherDashboardComponent implements OnInit {
 private fb = inject(FormBuilder);
 private teacherSvc = inject(TeacherService);
 private auth = inject(AuthService);
 private snack = inject(MatSnackBar);
 teacherId!: number;
 subjects: SubjectDto[] = [];
 addSubjectForm = this.fb.group({ name: ['', Validators.required] });
 marksForm = this.fb.group({
   studentId: [0, Validators.required],
   subjectName: ['', Validators.required],
   term: ['', Validators.required],
   marks: [0, Validators.required],
 });
 attendanceForm = this.fb.group({
   studentId: [0, Validators.required],
   date: ['', Validators.required],
   isPresent: [true, Validators.required]
 });
  router: any;
 ngOnInit(): void {
   const session = this.auth.getSession();
   if (session?.teacherId) {
     this.teacherId = session.teacherId;
     this.loadSubjects();
   }
 }
 loadSubjects() {
   if (!this.teacherId) return;
   this.teacherSvc.subjects(this.teacherId).subscribe({ next: s => this.subjects = s });
 }
 addSubject() {
   if (this.addSubjectForm.invalid || !this.teacherId) return;
   this.teacherSvc.addSubject(this.teacherId, { name: this.addSubjectForm.value.name! }).subscribe({
     next: s => { this.snack.open('Subject added', 'OK', { duration: 1200 }); this.addSubjectForm.reset(); this.loadSubjects(); },
     error: e => this.snack.open('Failed to add subject', 'Dismiss', { duration: 1800 })
   });
 }
 enterMarks() {
   if (this.marksForm.invalid || !this.teacherId) return;
   this.teacherSvc.enterMarks(this.teacherId, this.marksForm.getRawValue() as any).subscribe({
     next: _ => this.snack.open('Marks saved', 'OK', { duration: 1200 }),
     error: e => this.snack.open(e?.error ?? 'Failed to save marks', 'Dismiss', { duration: 2000 })
   });
 }
 markAttendance() {
   if (this.attendanceForm.invalid) return;
   this.teacherSvc.markAttendance(this.attendanceForm.getRawValue() as any).subscribe({
     next: _ => this.snack.open('Attendance saved', 'OK', { duration: 1200 }),
     error: e => this.snack.open('Failed to save attendance', 'Dismiss', { duration: 2000 })
   });
 }
}