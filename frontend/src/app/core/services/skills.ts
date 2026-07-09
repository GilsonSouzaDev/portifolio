import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum SkillCategory {
  Soft = 0,
  Hard = 1,
  Badge = 2,
}

export interface Skill {
  id: number;
  name: string;
  category: SkillCategory;
  description: string;
  proficiencyLevel: number;
  iconUrl: string | null;
  displayOrder: number;
}

@Injectable({
  providedIn: 'root',
})
export class SkillsService {
  private apiUrl = environment.apiUrl + '/skills';

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


