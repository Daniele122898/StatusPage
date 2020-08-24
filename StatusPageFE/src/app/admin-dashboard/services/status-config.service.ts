import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {StatusConfig} from '../models/StatusConfig';

@Injectable({
  providedIn: 'root'
})
export class StatusConfigService {

  private baseUrl = environment.apiUrl + 'config';

  constructor(
    private http: HttpClient
  ) { }

  public getStatusConfigs(): Observable<StatusConfig[]> {
    return this.http.get<StatusConfig[]>(this.baseUrl);
  }

}
