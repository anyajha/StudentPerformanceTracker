import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { StudentService } from '../../services/student.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.services';
import { MarksView, AttendanceView } from '../../models/student.models';
@Component({
 standalone: true,
 selector: 'app-student-dashboard',
 imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatFormFieldModule, MatInputModule],
 templateUrl: './student-dashboard.html',
 styleUrls:['./student-dashboard.scss']
})
export class StudentDashboardComponent implements OnInit {
 private studentSvc = inject(StudentService);
 private auth = inject(AuthService);
 private route=inject(ActivatedRoute)
 studentId!: number;
 marks: MarksView[] = [];
 attendance: AttendanceView[] = [];
 displayedMarks = ['subjectName', 'term', 'marks'];
 displayedAttendance = ['date', 'isPresent'];
  //route: any;
 

ngOnInit(): void {
   const p = this.route.snapshot.paramMap.get('id');
if (p) this.studentId = +p;
if (!this.studentId) this.studentId = this.auth.getSession()?.studentId ?? 0;
if (this.studentId) this.loadData();




 }
loadData() {
   if (!this.studentId) return;
   this.studentSvc.getMarks(this.studentId).subscribe({ next: d => this.marks = d });
   this.studentSvc.getAttendance(this.studentId).subscribe({ next: d => this.attendance = d });
 }
}  