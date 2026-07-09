import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Profile {
  id: number;
  name: string;
  title: string;
  bio: string;
  avatarUrl: string | null;
  resumeUrl: string | null;
}

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private apiUrl = environment.apiUrl + '/profile';

  constructor(private http: HttpClient) {}

  get(): Observable<Profile> {
    return this.http.get<Profile>(this.apiUrl);
  }

  update(profile: Profile): Observable<Profile> {
    return this.http.put<Profile>(this.apiUrl, profile);
  }
}


