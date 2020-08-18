import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ServiceStatus} from '../../../shared/models/ServiceStatus';

@Injectable({
  providedIn: 'root'
})
export class StatusService {

  private baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) { }

  public getServiceStatuses(): Observable<ServiceStatus[]> {
    return this.http.get<ServiceStatus[]>(this.baseUrl + 'status');
  }
}
