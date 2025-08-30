import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { AdminService } from '../../services/admin.service';
import { RouterLink } from '@angular/router';
@Component({
 standalone: true,
 selector: 'app-admin-dashboard',
 imports: [CommonModule, MatTableModule, MatButtonModule,],
 templateUrl: './admin-dashboard.html',
 styleUrls:['./admin-dashboard.scss']
})
export class AdminDashboardComponent {
 private adminSvc = inject(AdminService);
 students: any[] = [];
 teachers: any[] = [];
 categories: any;
 loadStudents() { this.adminSvc.studentsReport().subscribe({ next: d => this.students = d }); }
 loadTeachers() { this.adminSvc.teachersReport().subscribe({ next: d => this.teachers = d }); }
 loadCategories() { this.adminSvc.studentsCategories().subscribe({ next: d => this.categories = d }); }
}