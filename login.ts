import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AuthService } from '../../services/auth.services';
@Component({
 standalone: true,
 selector: 'app-login',
 imports: [
   CommonModule, ReactiveFormsModule, RouterLink,
   MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSnackBarModule
 ],
 templateUrl: './login.html',
 styleUrls:['./login.scss']
// styles: [`mat-card{max-width:520px;margin:40px auto;display:block}`]
})
export class LoginComponent {
 private fb = inject(FormBuilder);
 private auth = inject(AuthService);
 private router = inject(Router);
 private snack = inject(MatSnackBar);
 form = this.fb.group({
   username: ['', Validators.required],
   password: ['', Validators.required]
 });
 submitting = false;
 submit() {
   if (this.form.invalid) { this.form.markAllAsTouched(); return; }
   this.submitting = true;
   const body = this.form.getRawValue() as { username: string; password: string };
   this.auth.login(body).subscribe({
     next: (resp) => {
       this.submitting = false;
       this.auth.saveSession(resp);
       this.snack.open(`Welcome ${resp.name} (${resp.role})`, 'OK', { duration: 1200 });
       // ---- build target using the style in your screenshot ----
       const target: string | string[] =
         resp.role === 'Student' && resp.studentId != null ? ['/student', String(resp.studentId)] :
         resp.role === 'Teacher' && resp.teacherId != null ? ['/teacher', String(resp.teacherId)] :
         resp.role === 'Admin'                              ? ['/admin'] :
                                                              ['/student']; // fallback
       console.log('[Route] navigating to', target);
       // Normalize to a commands array for router.navigate
       const commands = Array.isArray(target) ? target : [target];
       // Ensure absolute path (first segment should start with '/')
       if (typeof commands[0] === 'string' && !commands[0].startsWith('/')) {
         commands[0] = '/' + commands[0];
       }
       // Try Angular Router first
       this.router.navigate(commands as any).then(ok => {
         console.log('[Route] navigate() result:', ok);
         if (!ok) this.hardRedirect(target);
       }).catch(() => this.hardRedirect(target));
     },
     error: (err) => {
       this.submitting = false;
       const msg = err?.error?.title || err?.error || err?.message || 'Login failed';
       this.snack.open(String(msg), 'Dismiss', { duration: 3000 });
       console.error('[login] error', err);
     }
   });
 }
 private hardRedirect(target: string | string[]) {
   // Build an ABSOLUTE URL so the browser never treats 'student' as a hostname
   const path =
     Array.isArray(target)
       ? '/' + target.filter(Boolean).join('/')     // -> '/student/1'
       : (target.startsWith('/') ? target : '/' + target);
   const absolute = `${window.location.origin}${path}`;
   console.warn('[Route] router failed, hard redirect to', absolute);
   window.location.assign(absolute);
 }
}