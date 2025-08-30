export interface SubjectDto { id: number; name: string; }
export interface SubjectCreateDto { name: string; }
export interface MarksEntryDto {
 studentId: number;
 subjectName: string;
 term: string;
 marks: number;
}
export interface AttendanceEntryDto {
 studentId: number;
 date: string;       // ISO string
 isPresent: boolean;
}