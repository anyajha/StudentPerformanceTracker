import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { enviroment } from '../enviroments/enviroments';
import { SignupRequest, LoginRequest, LoginResponse } from '../models/auth.models';
@Injectable({ providedIn: 'root' })
export class AuthService {
 private base = enviroment.apiBase;
 private readonly KEY= 'user';
 constructor(private http: HttpClient) {}

  // --- API calls ---

  signup(body: SignupRequest): Observable<LoginResponse> {

    return this.http.post<LoginResponse>(`${this.base}/auth/signup`, body);

  }

  login(body: LoginRequest): Observable<LoginResponse> {

    return this.http.post<LoginResponse>(`${this.base}/auth/login`, body);

  }

  // --- session helpers ---

  saveSession(resp: LoginResponse): void {

    localStorage.setItem(this.KEY, JSON.stringify(resp));

  }

  getSession(): LoginResponse | null {

    const raw = localStorage.getItem(this.KEY);

    return raw ? (JSON.parse(raw) as LoginResponse) : null;

  }

  logout(): void {

    localStorage.removeItem(this.KEY);

  }

  // --- convenience getters (optional) ---

  isLoggedIn(): boolean {

    return !!this.getSession();

  }

  role(): LoginResponse['role'] | null {

    return this.getSession()?.role ?? null;

  }

  studentId(): number | null {

    return this.getSession()?.studentId ?? null;

  }

  teacherId(): number | null {

    return this.getSession()?.teacherId ?? null;

  }

  adminId(): number | null {

    return this.getSession()?.adminId ?? null;

  }

}

