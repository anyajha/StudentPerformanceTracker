import { NgModule } from '@angular/core';

import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';


import { LoginComponent } from './components/login/login';
import { StudentDashboardComponent } from './components/student-dashboard/student-dashboard';
import { TeacherDashboardComponent } from './components/teacher-dashboard/teacher-dashboard';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard';
import { BrowserModule } from '@angular/platform-browser';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSortModule } from '@angular/material/sort';
import { AppComponent } from './app';
import { TeacherService } from './services/teacher.service';



const routes: Routes = [

  { path: '', component: LoginComponent },

  { path: 'student', component: StudentDashboardComponent },

  { path: 'teacher', component: TeacherDashboardComponent },

  { path: 'admin', component: AdminDashboardComponent }

];

// @NgModule({

// declarations:[
//     AppComponent,
//     AdminDashboardComponent,
//     TeacherDashboardComponent,
//     StudentDashboardComponent,
//     TeacherService
// ],
//   imports: [RouterModule.forRoot(routes),
//             BrowserModule,
//             ReactiveFormsModule,
//             MatTableModule,
//             MatPaginatorModule,
//             MatSortModule],

//   exports: [RouterModule]

// })

export class AppRoutingModule {}
 
