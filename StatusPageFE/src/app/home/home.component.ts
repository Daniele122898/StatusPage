import { Component, OnInit } from '@angular/core';
import {StatusService} from './services/status.service';
import {ServiceStatus} from '../../shared/models/ServiceStatus';
import {getStatusText} from '../../shared/models/Status';
import {SpecialNotice} from '../../shared/models/SpecialNotice';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public serviceStatuses: ServiceStatus[];
  public specialNotice: SpecialNotice;

  public getStatusText = getStatusText;

  constructor(
    private statusService: StatusService
  ) { }

  ngOnInit(): void {
    this.getAllStatuses();
  }

  private getAllStatuses(): void {
    this.statusService.getServiceStatuses().subscribe(
      statuses => {
        this.serviceStatuses = statuses;
      },
      err => {
        console.error(err);
      }
    );

    this.statusService.getSpecialNotice().subscribe(
      notice => {
        this.specialNotice = notice;
      }, err => {
        console.log(err);
      }
    );
  }
}
