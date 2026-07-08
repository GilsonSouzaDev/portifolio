import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Project {
  id: number;
  title: string;
  description: string;
  thumbnailUrl: string | null;
  projectUrl: string | null;
  repositoryUrl: string | null;
  technologies: string;
  featured: boolean;
  displayOrder: number;
}

@Injectable({
  providedIn: 'root',
})
export class ProjectsService {
  private apiUrl = '/api/projects';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Project[]> {
    return this.http.get<Project[]>(this.apiUrl);
  }

  update(id: number, project: Project): Observable<Project> {
    return this.http.put<Project>(`${this.apiUrl}/${id}`, project);
  }

  create(project: Partial<Project>): Observable<Project> {
    return this.http.post<Project>(this.apiUrl, project);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
