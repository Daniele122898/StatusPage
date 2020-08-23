import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {SpecialNotice} from '../../../shared/models/SpecialNotice';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class NoticeService {

  private baseUrl = environment.apiUrl + 'status';

  constructor(
    private http: HttpClient
  ) { }

  public getSpecialNotice(): Observable<SpecialNotice> {
    return this.http.get<SpecialNotice>(this.baseUrl + '/notice');
  }

  public setSpecialNotice(notice: SpecialNotice): Observable<any> {
    return this.http.post(this.baseUrl + '/notice', notice);
  }

  public removeSpecialNotice(): Observable<any> {
    return this.http.delete(this.baseUrl + '/notice');
  }
}
