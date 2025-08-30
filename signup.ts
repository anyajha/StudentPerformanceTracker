import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AuthService } from '../../services/auth.services';
import { SignupRequest, LoginResponse } from '../../models/auth.models';
@Component({
 standalone: true,
 selector: 'app-signup',
 imports: [CommonModule, RouterLink,ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatSnackBarModule],
 templateUrl: './signup.html',
 styleUrls:['./signup.scss']
// styles: [`mat-card{max-width:460px;margin:40px auto;display:block}`]
})
export class SignupComponent {
 private fb = inject(FormBuilder);
 private auth = inject(AuthService);
 private router = inject(Router);
 private snack = inject(MatSnackBar);
 roles = ['Student','Teacher','Admin'] as const;
 form = this.fb.group({
   name: ['', [Validators.required, Validators.minLength(2)]],
   username: ['', [Validators.required, Validators.minLength(3)]],
   password: ['', [Validators.required, Validators.minLength(4)]],
   role: ['Student', Validators.required]
 });
 submitting = false;
 submit() {
   if (this.form.invalid) { this.form.markAllAsTouched(); return; }
   this.submitting = true;
   const body = this.form.getRawValue() as SignupRequest;
   this.auth.signup(body).subscribe({
     next: (resp: LoginResponse) => {
       this.auth.saveSession(resp);
       this.snack.open(`Welcome ${resp.name} (${resp.role})!`, 'OK', { duration: 1500 });
       switch (resp.role) {
         case 'Student': this.router.navigateByUrl('/student'); break;
         case 'Teacher': this.router.navigateByUrl('/teacher'); break;
         case 'Admin':   this.router.navigateByUrl('/admin'); break;
       }
     },
     error: err => { this.snack.open(err?.error ?? 'Signup failed', 'Dismiss', { duration: 2500 }); this.submitting=false; },
     complete: () => this.submitting = false
   });
 }
}