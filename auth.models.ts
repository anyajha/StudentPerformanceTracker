export interface SignupRequest {
 username: string;
 password: string;
 role: 'Student' | 'Teacher' | 'Admin';
 name: string;
}
export interface LoginRequest { username: string; password: string; }
export interface LoginResponse {
 userId: number;
 username: string;
 role: 'Student' | 'Teacher' | 'Admin';
 name: string;
 studentId?: number | null;
 teacherId?: number | null;
 adminId?: number | null;
}