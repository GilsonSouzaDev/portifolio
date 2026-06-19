import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Skill {
  id: number;
  name: string;
  category: string;
  proficiencyLevel: number;
  iconUrl: string | null;
  displayOrder: number;
}

@Injectable({
  providedIn: 'root',
})
export class SkillsService {
  private apiUrl = 'http://localhost:5217/api/skills';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Skill[]> {
    return this.http.get<Skill[]>(this.apiUrl);
  }

  update(id: number, skill: Skill): Observable<Skill> {
    return this.http.put<Skill>(`${this.apiUrl}/${id}`, skill);
  }

  create(skill: Partial<Skill>): Observable<Skill> {
    return this.http.post<Skill>(this.apiUrl, skill);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
