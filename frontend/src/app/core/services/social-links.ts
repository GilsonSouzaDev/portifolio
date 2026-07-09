import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SocialLink {
  id: number;
  platform: string;
  url: string;
  iconUrl: string | null;
  displayOrder: number;
}

@Injectable({
  providedIn: 'root',
})
export class SocialLinksService {
  private apiUrl = environment.apiUrl + '/social-links';

  constructor(private http: HttpClient) {}

  getAll(): Observable<SocialLink[]> {
    return this.http.get<SocialLink[]>(this.apiUrl);
  }

  update(id: number, link: SocialLink): Observable<SocialLink> {
    return this.http.put<SocialLink>(`${this.apiUrl}/${id}`, link);
  }
}


