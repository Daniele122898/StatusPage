import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable} from 'rxjs';
import {StatusConfig} from '../models/StatusConfig';

@Injectable({
  providedIn: 'root'
})
export class StatusConfigService {

  private baseUrl = environment.apiUrl + 'config';

  private listRefreshSubj = new BehaviorSubject<void>(null);
  public listRefreshObs = this.listRefreshSubj.asObservable();

  constructor(
    private http: HttpClient
  ) { }

  public createStatusConfig(config: StatusConfig): Observable<any> {
    return this.http.post<any>(this.baseUrl, config);
  }

  public editStatusConfig(oldId: string, newConfig: StatusConfig): Observable<any> {
    return this.http.post<any>(this.baseUrl + `/${oldId}`, newConfig);
  }

  public getStatusConfigs(): Observable<StatusConfig[]> {
    return this.http.get<StatusConfig[]>(this.baseUrl);
  }

  public removeConfig(identifier: string): Observable<any> {
    return this.http.delete(this.baseUrl + '/' + identifier);
  }

  public forceListRefresh(): void {
    this.listRefreshSubj.next(null);
  }

}
