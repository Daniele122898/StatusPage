import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ServiceStatus} from '../../../shared/models/ServiceStatus';
import {SpecialNotice} from '../../../shared/models/SpecialNotice';

@Injectable({
  providedIn: 'root'
})
export class StatusService {

  private baseUrl = environment.apiUrl + 'status';

  constructor(
    private http: HttpClient
  ) { }

  public getServiceStatuses(): Observable<ServiceStatus[]> {
    return this.http.get<ServiceStatus[]>(this.baseUrl);
  }

  public getSpecialNotice(): Observable<SpecialNotice> {
    return this.http.get<SpecialNotice>(this.baseUrl + '/notice');
  }
}
