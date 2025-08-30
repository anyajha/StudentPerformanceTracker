import { Routes } from '@angular/router';
// Import components directly

import { LoginComponent } from './components/login/login';
import { SignupComponent } from './components/signup/signup';
import { StudentDashboardComponent } from './components/student-dashboard/student-dashboard';
import { TeacherDashboardComponent } from './components/teacher-dashboard/teacher-dashboard';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard';

export const routes: Routes = [
 { path: '', redirectTo: 'login', pathMatch: 'full' },
 { path: 'login', component: LoginComponent },
 { path: 'signup', component: SignupComponent },
 // ✅ accept optional id
 { path: 'student', component: StudentDashboardComponent },
 { path: 'student/:id', component: StudentDashboardComponent },
 { path: 'teacher', component: TeacherDashboardComponent },
 { path: 'teacher/:id', component: TeacherDashboardComponent },
 { path: 'admin', component: AdminDashboardComponent },
// { path: '**', redirectTo: 'login' }
];